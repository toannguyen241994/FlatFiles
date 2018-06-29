﻿using System;
using FlatFiles.Properties;

namespace FlatFiles
{
    /// <summary>
    /// Represents an error that occurred while parsing a stream.
    /// </summary>
    public class FlatFileException : Exception
    {
        /// <summary>
        /// Initializes a new instance of a FlatFileException, recording which record caused the error.
        /// </summary>
        /// <param name="message">A message describing the cause of the error.</param>
        internal FlatFileException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of a FlatFileException.
        /// </summary>
        /// <param name="message">A message describing the cause of the error.</param>
        /// <param name="innerException">An inner exception containing the cause of the underlying error.</param>
        internal FlatFileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Represents an error that was thrown while parsing a column value.
    /// </summary>
    public sealed class ColumnProcessingException : FlatFileException
    {
        internal ColumnProcessingException(IColumnContext context, string value, Exception innerException)
            : base(GetErrorMessage(context, value), innerException)
        {
            ColumnContext = context;
            ColumnValue = value;
        }

        private static string GetErrorMessage(IColumnContext context, string value)
        {
            var position = context.PhysicalIndex;
            var definition = context.ColumnDefinition;
            var message = String.Format(null, Resources.InvalidColumnConversion, value, definition.ColumnType.FullName, definition.ColumnName, position);
            return message;
        }

        /// <summary>
        /// Gets the schema that was being used to parse when the error occurred.
        /// </summary>
        public IColumnContext ColumnContext { get; internal set; }

        /// <summary>
        /// Gets the value that was being parsed when the error occurred.
        /// </summary>
        public string ColumnValue { get; }
    }

    /// <summary>
    /// Represents an error that was thrown while parsing a record.
    /// </summary>
    public sealed class RecordProcessingException : FlatFileException
    {
        internal RecordProcessingException(IRecordContext context, string message)
            : base(String.Format(null, message, context.PhysicalRecordNumber))
        {
            Context = context;
        }

        internal RecordProcessingException(IRecordContext context, string message, Exception innerException)
            : base(String.Format(null, message, context.PhysicalRecordNumber), innerException)
        {
            Context = context;
        }

        /// <summary>
        /// Gets the metadata for the record being processed when the error occurred.
        /// </summary>
        public IRecordContext Context { get; }
    }
}
