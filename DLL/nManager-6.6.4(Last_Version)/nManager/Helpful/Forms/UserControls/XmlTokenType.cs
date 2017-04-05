namespace nManager.Helpful.Forms.UserControls
{
    using System;

    public enum XmlTokenType
    {
        Whitespace,
        XmlDeclarationStart,
        XmlDeclarationEnd,
        NodeStart,
        NodeEnd,
        NodeEndValueStart,
        NodeName,
        NodeValue,
        AttributeName,
        AttributeValue,
        EqualSignStart,
        EqualSignEnd,
        CommentStart,
        CommentValue,
        CommentEnd,
        CDataStart,
        CDataValue,
        CDataEnd,
        DoubleQuotationMarkStart,
        DoubleQuotationMarkEnd,
        SingleQuotationMarkStart,
        SingleQuotationMarkEnd,
        DocTypeStart,
        DocTypeName,
        DocTypeDeclaration,
        DocTypeDefStart,
        DocTypeDefValue,
        DocTypeDefEnd,
        DocTypeEnd,
        DocumentEnd,
        Unknown
    }
}

