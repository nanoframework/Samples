using System;
using System.Collections;
using System.Threading;

namespace Collections
{
    public class Program
    {
        public static void Main()
        {
            string key1 = "key1";
            string key2 = "key2";
            string key3 = "key3";
            string key4 = "key4";
            string key5 = "key5";

            string[] keys = new string[] { key1, key2, key3, key4, key5 };

            var entry1 = Guid.NewGuid();
            var entry2 = Guid.NewGuid();
            var entry3 = Guid.NewGuid();
            var entry4 = Guid.NewGuid();
            var entry5 = Guid.NewGuid();

            ArrayList entries = new ArrayList { entry1, entry2, entry3, entry4, entry5 };

            Hashtable t = new Hashtable();

            // 1) add 5 items with 5 unique keys
            t.Add(key1, entry1);
            t.Add(key2, entry2);
            t.Add(key3, entry3);
            t.Add(key4, entry4);
            t.Add(key5, entry5);

            // 2) check all added keys are present
            if (
               !t.Contains(key1) ||
               !t.Contains(key2) ||
               !t.Contains(key3) ||
               !t.Contains(key4) ||
               !t.Contains(key5)
              )
            {
                throw new Exception("missing key");
            }


            // 3) check that the items are what they are expected to be
            // check the items reference and value first...
            int index = 0;
            foreach (string k in keys)
            {
                // test indexer
                var entry = t[k];

                //// check that the reference is the same 
                //if (!Object.ReferenceEquals(entry, (entries[index])))
                //{
                //    throw new Exception("reference check failed");
                //}

                // check that the values are the same
                if (!entry.Equals(entries[index]))
                {
                    throw new Exception("value check failed");
                }
                index++;
            }
            // ... then check the keys                
            foreach (String k in keys)
            {
                bool found = false;
                ICollection keysCollection = t.Keys;
                foreach (string key in keysCollection)
                {
                    if (k == key)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("key/value pair check failed");
                }
            }

            // 4) checked that we can remove the items
            // ... then check the keys                
            foreach (String k in keys)
            {
                t.Remove(k);
            }

            // 4) checked that we can remove the items
            // ... then check the keys                
            foreach (String k in keys)
            {
                t.Remove(k);
            }

            // 5) check that a removed item is no longer in the table, and so its key it is no longer in the table as well           
            // check the items reference and value first...
            // test nothing is left in the Hashtable 
            if (t.Count != 0)
            {
                throw new Exception("there are still itens in the collection");
            }

            int indexR = 0;
            foreach (String k in keys)
            {
                // test Contains
                if (t.Contains(k))
                {
                    throw new Exception("removed item is still on collection");
                }

                // test indexer
                var entry = t[k];
                if (entry != null)
                {
                    throw new Exception("item should not exist");
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
