using System.ComponentModel.DataAnnotations;

namespace twe_todo_app.Models.Keys {
    public class ConsumerKeys {
        [Key]
        public string Key { get; set; }
        public string Secret { get; set; }
    }
}
