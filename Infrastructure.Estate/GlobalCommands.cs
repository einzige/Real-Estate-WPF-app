using System;
using System.Collections.Generic;

namespace Infrastructure.Estate
{
    using Microsoft.Practices.Composite.Presentation.Commands;

    public static class GlobalCommands
    {
        public static CompositeCommand AddNewRibbonModuleCommand = new CompositeCommand();
        public static CompositeCommand AddNewTabCommand = new CompositeCommand();
        public static CompositeCommand TestCommand = new CompositeCommand();
        public static CompositeCommand RefreshReferenceBookCommand = new CompositeCommand();
    }

    public class GlobalCommandsProxy
    {
        virtual public CompositeCommand AddNewRibbonModuleCommand
        {
            get { return GlobalCommands.AddNewRibbonModuleCommand; }
        }

        virtual public CompositeCommand AddNewTabCommand
        {
            get { return GlobalCommands.AddNewTabCommand; }
        }

        virtual public CompositeCommand TestCommand
        {
            get { return GlobalCommands.TestCommand; }
        }

        virtual public CompositeCommand RefreshReferenceBookCommand
        {
            get { return GlobalCommands.RefreshReferenceBookCommand; }
        }
    }

    public static class CommandTypes
    {
        public const string Insert = "Insert";
        public const string Update = "Update";
        public const string Delete = "Delete";
    }

    public class CommandParameter
    {
        public long   CollectionID { get; set; } // Collection has a reference, which defines accessories to the parent collection (physically it is Reference ID)
        public string ObjectClass  { get; set; } // TableName
        public object Object       { get; set; } // Row
        public object Sender       { get; set; } // ViewModel that generates params
        public string CommandType  { get; set; } // SQL INSERT< UPDATE< ETC desctibed into CommandTypes
    }


}
