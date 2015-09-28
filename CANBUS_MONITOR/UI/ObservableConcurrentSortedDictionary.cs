/* .
 */

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace CANBUS_MONITOR
{
    [Serializable]
    public class ObservableConcurrentSortedDictionary<TKey, TValue> : ObservableConcurrentDictionary<TKey, TValue>, ISerializable, IDeserializationCallback
    {
        #region constructors

        #region public

        public ObservableConcurrentSortedDictionary(IComparer<DictionaryEntry> comparer)
            : base()
        {
            _comparer = comparer;
        }
        /*
        public ObservableSortedDictionary(IComparer<DictionaryEntry> comparer, IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            _comparer = comparer;
        }

        public ObservableSortedDictionary(IComparer<DictionaryEntry> comparer, IEqualityComparer<TKey> equalityComparer)
            : base(equalityComparer)
        {
            _comparer = comparer;
        }

        public ObservableSortedDictionary(IComparer<DictionaryEntry> comparer, IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TKey> equalityComparer)
            : base(dictionary, equalityComparer)
        {
            _comparer = comparer;
        }

        protected ObservableSortedDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _siInfo = info;
        }
        */
        #endregion public

        #endregion constructors

        #region methods

        #region protected

        protected override bool AddEntry(TKey key, TValue value)
        {
            DictionaryEntry entry = new DictionaryEntry(key, value);
            int index = GetInsertionIndexForEntry(entry);
            _keyedEntryCollection.Insert(index, entry);
            return true;
        }

        protected virtual int GetInsertionIndexForEntry(DictionaryEntry newEntry)
        {
            return BinaryFindInsertionIndex(0, Count - 1, newEntry);
        }

        protected virtual bool SetEntry(TKey key, TValue value)
        {
            bool keyExists = _keyedEntryCollection.Contains(key);

            // if identical key/value pair already exists, nothing to do
            if (keyExists && value.Equals((TValue)_keyedEntryCollection[key].Value))
                return false;

            // otherwise, remove the existing entry
            if (keyExists)
                _keyedEntryCollection.Remove(key);

            // add the new entry
            DictionaryEntry entry = new DictionaryEntry(key, value);
            int index = GetInsertionIndexForEntry(entry);
            _keyedEntryCollection.Insert(index, entry);

            return true;
        }

        #endregion protected

        #region private

        private int BinaryFindInsertionIndex(int first, int last, DictionaryEntry entry)
        {
            if (last < first)
            {

                return first;
            }
            else
            {
                int mid = first + (int)((last - first) / 2);
                int result = _comparer.Compare(_keyedEntryCollection[mid], entry);

                if (result == 0)
                    return mid;
                else if (result < 0)
                    return BinaryFindInsertionIndex(mid + 1, last, entry);
                else
                    return BinaryFindInsertionIndex(first, mid - 1, entry);
            }
        }

        #endregion private

        #endregion methods

        #region interfaces

        #region ISerializable


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            if (!_comparer.GetType().IsSerializable)
            {
                throw new NotSupportedException("The supplied Comparer is not serializable.");
            }

            base.GetObjectData(info, context);
            info.AddValue("_comparer", _comparer);
        }

        #endregion ISerializable

        #region IDeserializationCallback

        public override void OnDeserialization(object sender)
        {
            if (_siInfo != null)
            {
                _comparer = (IComparer<DictionaryEntry>)_siInfo.GetValue("_comparer", typeof(IComparer<DictionaryEntry>));
            }
            base.OnDeserialization(sender);
        }

        #endregion IDeserializationCallback

        #endregion interfaces

        #region fields

        private IComparer<DictionaryEntry> _comparer;

        [NonSerialized]
        private SerializationInfo _siInfo = null;

        #endregion fields
    }

}