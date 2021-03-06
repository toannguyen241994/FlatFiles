﻿using System;

namespace FlatFiles.TypeMapping
{
    /// <summary>
    /// Represents the mapping from a type property to a char column.
    /// </summary>
    public interface ICharPropertyMapping
    {
        /// <summary>
        /// Sets the name of the column in the input or output file.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The property mapping for further configuration.</returns>
        ICharPropertyMapping ColumnName(string name);

        /// <summary>
        /// Sets whether the parser should ignore extra characters.
        /// </summary>
        /// <param name="allow">True if the parser should ignore extra characters -or- false, if an error should occur.</param>
        /// <returns>The property mapping for further configuration.</returns>
        ICharPropertyMapping AllowTrailing(bool allow);

        /// <summary>
        /// Sets the value to treat as null.
        /// </summary>
        /// <param name="value">The value to treat as null.</param>
        /// <returns>The property mapping for further configuration.</returns>
        ICharPropertyMapping NullValue(string value);

        /// <summary>
        /// Sets a custom handler for nulls.
        /// </summary>
        /// <param name="handler">The handler to use to recognize nulls.</param>
        /// <returns>The property mapping for further configuration.</returns>
        /// <remarks>Setting the handler to null with use the default handler.</remarks>
        ICharPropertyMapping NullHandler(INullHandler handler);

        /// <summary>
        /// Sets a function to preprocess in the input before parsing it.
        /// </summary>
        /// <param name="preprocessor">A preprocessor function.</param>
        /// <returns>The property mapping for further configuration.</returns>
        ICharPropertyMapping Preprocessor(Func<string, string> preprocessor);
    }

    internal sealed class CharPropertyMapping : ICharPropertyMapping, IMemberMapping
    {
        private readonly CharColumn column;

        public CharPropertyMapping(CharColumn column, IMemberAccessor member, int physicalIndex, int logicalIndex)
        {
            this.column = column;
            Member = member;
            PhysicalIndex = physicalIndex;
            LogicalIndex = logicalIndex;
        }

        public ICharPropertyMapping ColumnName(string name)
        {
            column.ColumnName = name;
            return this;
        }

        public ICharPropertyMapping AllowTrailing(bool allow)
        {
            column.AllowTrailing = allow;
            return this;
        }

        public ICharPropertyMapping NullValue(string value)
        {
            column.NullHandler = new ConstantNullHandler(value);
            return this;
        }

        public ICharPropertyMapping NullHandler(INullHandler handler)
        {
            column.NullHandler = handler;
            return this;
        }

        public ICharPropertyMapping Preprocessor(Func<string, string> preprocessor)
        {
            column.Preprocessor = preprocessor;
            return this;
        }

        public IMemberAccessor Member { get; }

        public Action<IColumnContext, object, object> Reader => null;

        public Action<IColumnContext, object, object[]> Writer => null;

        public IColumnDefinition ColumnDefinition => column;

        public int PhysicalIndex { get; }

        public int LogicalIndex { get; }
    }
}
