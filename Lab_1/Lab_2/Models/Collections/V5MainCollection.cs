using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("WpfApp")]
namespace Lab_2.Models.Collections
{
    [Serializable]
    public class V5MainCollection : IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public List<V5Data> V5List { get; set; }
        public bool Change { get; set; }
        public V5MainCollection()
        {
            V5List = new List<V5Data>();
        }

        public string ErrorMessage { get; set; }

        public int Count()
        {
            return V5List.Count;
        }
        public V5Data this[int index]
        {
            get { return V5List[index]; }
            set { V5List[index] = value; DataChanged?.Invoke(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, V5List[index].info)); }
        }
        public event DataChangedEventHandler DataChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void IsCollectionChanged(NotifyCollectionChangedAction ev)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveAt(int index)
        {
            V5List.RemoveAt(index);
            OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);
        }

        public IEnumerable<DataItem> Ditems
        {
            get
            {
                var query1 = from elem in (from data in V5List where data is V5DataCollection select (V5DataCollection)data)
                             from item in elem
                             select item;
                var query = query1.Union(from elem in (from data in V5List where data is V5DataOnGrid select (V5DataOnGrid)data)
                                         from item in elem
                                         select item);
                return from elem in query orderby elem.val.Length() select elem;
            }
        }

        public string Error { get; set; }

        public void AddDefaultDataCollection() {
            V5DataCollection DC = new V5DataCollection("DC");
            DC.InitRandom(2, 5, 5, 0, 5);
            Add(DC);
        }

        public IEnumerator GetEnumerator() {
            return V5List.GetEnumerator();
        }

        protected virtual void OnCollectionChanged(object source, NotifyCollectionChangedAction action)
        {
            CollectionChanged?.Invoke(source, new NotifyCollectionChangedEventArgs(action));
        }

        public void Add(V5Data item)
        {
            item.PropertyChanged += OnPropertyChanged;
            V5List.Add(item);
            DataChanged?.Invoke(this, new DataChangedEventArgs(ChangeInfo.Add, item.info));
        }
        void OnPropertyChanged(object source, PropertyChangedEventArgs args)
        {
            Console.WriteLine(args.PropertyName + " changed");
            DataChanged?.Invoke(this, new DataChangedEventArgs(ChangeInfo.Replace, ((V5Data)source).info));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void IsPropertyChanged(string ch = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ch));
        }

        public bool Remove(string id, DateTime d)
        {
            bool flag = false;
            for (int i = 0; i < V5List.Count; i++)
            {
                if (String.Equals(V5List[i].info, id) == true
                        && V5List[i].date.CompareTo(d) == 0)
                {
                    V5List[i].PropertyChanged -= OnPropertyChanged;
                    DataChanged?.Invoke(this, new DataChangedEventArgs(ChangeInfo.Remove, V5List[i].info));
                    V5List.RemoveAt(i);
                    i--;
                    flag = true;
                }
            }
            return flag;
        }

        public void RemoveAll()
        {
            V5List.Clear();
            OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);
        }

        public void AddFromFile(string filename)
        {
            try
            {
                V5DataOnGrid DG = new V5DataOnGrid(filename);
                Add(DG);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddDefaults()
        {
            Random rand = new Random(123);
            int NElem = rand.Next(2, 11), Rand1, Rand2;
            Grid2D Gr;
            V5DataCollection DataColl;
            V5DataOnGrid DataGrid;
            V5List = new List<V5Data>();
            for (int i = 0; i < NElem; i++)
            {
                Rand1 = rand.Next(0, 2);
                Gr = new Grid2D(2, 2, 2, 2);
                if (Rand1 == 0)
                {
                    DataGrid = new V5DataOnGrid("DG", DateTime.Now, Gr);
                    DataGrid.InitRandom(0, 10);
                    Add(DataGrid);
                }
                else
                {
                    Rand2 = rand.Next(1, 10);
                    DataColl = new V5DataCollection("DC", DateTime.Now);
                    DataColl.InitRandom(Rand2, 2, 5, 2, 3);
                    Add(DataColl);
                }
            }
        }

        public override string ToString()
        {
            string str = "";
            foreach (V5Data item in V5List)
            {
                str += item.ToString();
            }
            str += "\n";
            return str;
        }

        public string ToLongString(string format)
        {
            string str = "";
            foreach (V5Data item in V5List)
            {
                str += item.ToString(format);
            }
            return str;
        }

        public float MaxDistance(Vector2 v)
        {
            var query1 = from elem in (from data in V5List where data is V5DataCollection select (V5DataCollection)data)
                         from item in elem
                         select Vector2.Distance(v, item.coord);
            var query2 = from elem in (from data in V5List where data is V5DataOnGrid select (V5DataOnGrid)data)
                         from item in elem
                         select Vector2.Distance(v, item.coord);
            float MaxDC = 0;
            float MaxDG = 0;
            if (query1 != null)
                MaxDC = query1.Max();
            if (query2 != null)
                MaxDG = query2.Max();
            if (MaxDC > MaxDG)
                return MaxDC;
            else
                return MaxDG;
        }

        public IEnumerable<DataItem> MaxDistanceItems(Vector2 v)
        {
            var query1 = from elem in (from data in V5List where data is V5DataCollection select (V5DataCollection)data)
                         from item in elem
                         where Vector2.Distance(v, item.coord) == MaxDistance(v)
                         select item;
            return query1.Union(from elem in (from data in V5List where data is V5DataOnGrid select (V5DataOnGrid)data)
                                from item in elem
                                where Vector2.Distance(v, item.coord) == MaxDistance(v)
                                select item);
        }

        public void Save(string filename)
        {
            FileStream fs = null;
            try {
                fs = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, this);
                Change = false;
            }
            catch (Exception) { Console.WriteLine("Save error");}
            finally { if (fs != null) fs.Close(); }
        }

        public void Load(string filename) {
            FileStream fs = null;
            try {
                fs = File.OpenRead(filename);
                BinaryFormatter bf = new BinaryFormatter();
                List<V5Data> l = bf.Deserialize(fs) as List<V5Data>;
                V5List = l;
            }
            catch (Exception) { Error = "Loading error";}
            finally {
                Change = true;
                if (fs != null) fs.Close();
                IsCollectionChanged(NotifyCollectionChangedAction.Add);
                IsPropertyChanged("change");
            }
        }

        public void AddDefaultDataOnGrid() {
            Grid2D grid = new Grid2D();
            V5DataOnGrid DoG = new V5DataOnGrid("Default DoG", default, grid);
            DoG.InitRandom(0, 10);
            Add(DoG);
        }
    }
}
