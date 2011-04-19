using System.Collections.Generic;

namespace ReferenceBook
{
    using System.ComponentModel;
    using System.Collections.ObjectModel;

    public class EntityCollection : ObservableCollection<IEntityViewModel>
    {
        public event ItemEndEditEventHandler ItemEndEdit; // some item from the collection has been changed

        public EntityCollection() { }

        public EntityCollection(List<IEntity> list)
        {
            foreach (IEntity e in list)
            {
                Add(new EntityViewModel(e));
            }
        }

        protected override void InsertItem(int index, IEntityViewModel item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }

        void ItemEndEditHandler(IEditableObject sender)
        {
            if (ItemEndEdit != null) ItemEndEdit(sender);
        }
    }
}
