﻿using System;

namespace  WPM_API.Data.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public Type RecordType { get; }
        public string RecordId { get; }
        public bool ExistsAsDeleted { get; }

        public RecordNotFoundException(Type recordType, string recordId, bool existsAsDeleted = false)
            : base(GetFormattedMessage(recordType, recordId))
        {
            RecordType = recordType;
            RecordId = recordId;
            ExistsAsDeleted = existsAsDeleted;
        }

        private static string GetFormattedMessage(Type recordType, string recordId)
        {
            return $"{recordType.Name} not found. ID - {recordId}";
        }
    }
}
