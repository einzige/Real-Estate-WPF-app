namespace ReferenceBook
{
    using Microsoft.Practices.Composite.Presentation.Commands;

    public static class DatabaseDirectoryCommands
    {
        static CompositeCommand _setDictionaryTableCommand;
        public static CompositeCommand SetDictionaryTableCommand
        {
            get
            {
                if (_setDictionaryTableCommand == null)
                    _setDictionaryTableCommand = new CompositeCommand();
                return _setDictionaryTableCommand;
            }
        }
        static CompositeCommand _showRegionsEditorCommand;
        public static CompositeCommand ShowRegionsEditor
        {
            get
            {
                if (_showRegionsEditorCommand == null)
                    _showRegionsEditorCommand = new CompositeCommand();
                return _showRegionsEditorCommand;
            }
        }
        static CompositeCommand _initAddingItemCommand;
        public static CompositeCommand InitAddingItem
        {
            get
            {
                if (_initAddingItemCommand == null)
                    _initAddingItemCommand = new CompositeCommand();
                return _initAddingItemCommand;
            }
        }
        static CompositeCommand _initEditingItemCommand;
        public static CompositeCommand InitEditingItem
        {
            get
            {
                if (_initEditingItemCommand == null)
                    _initEditingItemCommand = new CompositeCommand();
                return _initEditingItemCommand;
            }
        }

    }
}