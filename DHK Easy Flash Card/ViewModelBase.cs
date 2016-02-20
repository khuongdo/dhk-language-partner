using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DHK_Easy_Flash_Card
{
    public class ViewModelBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, args);
        }
    }
    public abstract class ViewModelBaseWithArgCache : ViewModelBase
    {
        private readonly Dictionary<string, PropertyChangedEventArgs> eventArgsCache;

        protected ViewModelBaseWithArgCache()
        {
            eventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();
        }

        #region Overrides

        protected override void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args;
            if (!eventArgsCache.ContainsKey(propertyName))
            {
                args = new PropertyChangedEventArgs(propertyName);
                eventArgsCache.Add(propertyName, args);
            }
            else
            {
                args = eventArgsCache[propertyName];
            }

            OnPropertyChanged(args);
        }

        #endregion

    }
}
