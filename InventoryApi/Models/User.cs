using System.ComponentModel.DataAnnotations;

namespace InventoryApi
{
    /**
     * 
      For Refference Only. User class/ model.
     * 
     */

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int UserHomeStore { get; set; }
    }
}