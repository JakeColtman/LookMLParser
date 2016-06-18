import sqlparse

class SQL:
    def __init__(self, sql_text):
        self.sql_text = sql_text
        self.columns = self._extract_columns_from_sql(sql_text)

    def _extract_columns_from_sql(self, text):

        def parse_column(column_text):
            if type(column_text) == sqlparse.sql.Identifier and column_text.has_alias():
                return column_text.get_alias()
            elif type(column_text) == sqlparse.sql.Identifier:
                return column_text.get_name()
            else:
                return str(column_text)

        tokens = sqlparse.parse(text)[0].tokens
        state = "start"
        columns = []
        tokens = [x for x in tokens if not (x.ttype is sqlparse.tokens.Whitespace)]
        for token in tokens:

            if type(token) is sqlparse.sql.Comment:
                continue

            if token.ttype is sqlparse.tokens.DML:
                state = "select"
                continue

            if state == "select" and token.ttype is not sqlparse.tokens.Punctuation:
                if token.value.lower() == "from":
                    state = "from"
                    continue

                if type(token) is sqlparse.sql.IdentifierList:
                    for item in token.get_identifiers():
                        columns.append(parse_column(item))
                    continue
                if token.ttype is not sqlparse.tokens.Punctuation and token.ttype is not sqlparse.tokens.Whitespace and str(
                        token.value) != "\n":
                    columns.append(parse_column(token))

            if state == "from":
                break

        return columns

    def _extract_final_query(self, text):

        tokens = sqlparse.parse(text)[0].tokens
        if not any([x.ttype is sqlparse.tokens.Keyword and "with" in x.value for x in tokens]):
            return text

        ii = 0
        while ii < len(tokens):
            if type(tokens[ii]) == sqlparse.sql.IdentifierList:
                break
            if type(tokens[ii]) == type(tokens[ii]) == sqlparse.sql.Identifier:
                break
            if tokens[ii].ttype is sqlparse.tokens.DML:
                raise
            ii += 1

        final_query_text = "".join([str(x) for x in tokens[ii + 1:]])
        return final_query_text

