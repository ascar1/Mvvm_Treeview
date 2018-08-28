using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLight1.Model
{
    public class LavelViewModel :ViewModelBase
    {
        private readonly LavelModel _lavelModel;
        private LavelViewModel _parent;
        public LavelViewModel()
        {

        }
        public LavelViewModel (LavelModel model)
        {
            _lavelModel = model;

            Children = new ObservableCollection<LavelViewModel>();
        }
        public int ID
        {
            get => _lavelModel.id;
            set => _lavelModel.id = value;
        }
        public int ParentID
        {
            get
            {
                return _lavelModel.paremtId;
            }
            set
            {
                _lavelModel.paremtId = value;
            }
        }
        public string name {
            get
            {
                return _lavelModel.name;
            }
            set
            {
                _lavelModel.name = value;
            }
        }
        public bool isRoot
        {
            get { return _lavelModel.paremtId == -1; }
        }
        public LavelViewModel parent {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                if (value != null)
                {
                    ParentID = value.ID;
                }
            }
        }
        public ObservableCollection<LavelViewModel> Children { get; set; }

        public static void setChild (LavelViewModel root, IList<LavelModel> source)
        {
            for (var i = 0; i < source.Count; i++)
            {
                if (root.ID != source[i].id && root.ID == source[i].paremtId)
                {
                    if (source[i].paremtId != -1)
                    {
                        //source[i].Parent = root;
                        LavelViewModel tmp = new LavelViewModel(source[i]);
                        root.Children.Add(tmp);
                        setChild(tmp, source);
                    }
                }
            }
        }

    }


}

