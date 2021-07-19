using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace OrderAPI.Infrastructure.Core
{
    public class DynamodbGuidToStringConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            return value.ToString();
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            Primitive primitive = entry as Primitive;
            if (primitive == null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
                throw new ArgumentOutOfRangeException();

            string value = (string)primitive.Value;

            return new Guid(value);
        }
    }
}