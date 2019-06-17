using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ParallelProjectTypes
{
    class BaseOptionsModel<T>
    {
        private ShellSettingsManager settingsManager;
        public BaseOptionsModel(ShellSettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }
        protected virtual string CollectionName { get; } = typeof(T).FullName;

        
        public void SaveSettings()
        {
            var settingsStore = this.settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            foreach (PropertyInfo property in GetOptionProperties())
            {
                string output = TypeDescriptor.GetConverter(property.GetType()).ConvertToString(property.GetValue(this));
                settingsStore.SetString(CollectionName, property.Name, output);
            }
        }

        public void LoadSettings()
        {
            var settingsStore = this.settingsManager.GetReadOnlySettingsStore(SettingsScope.UserSettings);

            foreach (PropertyInfo property in GetOptionProperties())
            {
                string input = settingsStore.GetString(CollectionName, property.Name);
                object value = TypeDescriptor.GetConverter(property.GetType()).ConvertFromString(input);
                property.SetValue(this, value);
            }
        }

        private IEnumerable<PropertyInfo> GetOptionProperties()
        {
            return GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsSerializable && p.PropertyType.IsPublic);
        }
    }

}
