using UnityEngine;
using System.Collections;

public class DBWhere
{
    private string m_columnName;
    public string ColumnName { get { return m_columnName; } }

    private string m_comparisonOperator;
    public string ComparisonOperator { get { return m_comparisonOperator; } }

    private string m_whereArg;
    public string WhereArg { get { return m_whereArg; } }

    public DBWhere(string columnName, string comparisonOperator, string whereArg)
    {
        m_columnName = columnName;
        m_comparisonOperator = comparisonOperator;
        m_whereArg = whereArg;
    }
}
