namespace BaseLibrary.Domain.Audit
{
    public class RecordFieldChanges
    {
        public List<EventChange> EventChanges { get; set; }

        public RecordFieldChanges()
        {
            EventChanges = new List<EventChange>();
        }

        public void Add(string field, string newValue, string oldValue = "")
        {
            if (string.IsNullOrWhiteSpace(field)) return;
            if (string.IsNullOrWhiteSpace(newValue)) newValue = "";
            if (string.IsNullOrWhiteSpace(oldValue)) oldValue = "";
            if (newValue.Trim() == oldValue.Trim()) return;
            EventChanges.Add(new EventChange(field, newValue, oldValue));
        }

        public string GetJSON()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }

        public static RecordFieldChanges? GetRecordFieldChanges(string json)
        {
            return JsonConvert.DeserializeObject<RecordFieldChanges>(json);
        }

        public bool HasChanges()
        {
            return EventChanges.Any();
        }
    }

    public class EventChange
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public EventChange(string field, string newValue, string oldValue = "")
        {
            FieldName = field;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
