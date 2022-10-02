using System.ComponentModel.DataAnnotations;

namespace InventoryApi
{
    /**
     * 
        For nomenclature, a model is a specific kind of part (Manufacturer/Model Number), 
        while an item is a physical part in inventory that is identified by model.
        Models are child elements of a “part category”, e.g. transmission, brakes, engine, etc.
     * 
     */

    public class Inventory
    {
        // Items are stored here, used to refill local stores
        public int WarehouseId { get; set; }

        //Store that users pull parts from
        public int StoreId { get; set; }
        
        [Required]
        [Key]
        //Item ID KEY value
        public int ModelId { get; set; }

        //NMN - description of item
        public string Nomenclature { get; set; }

        //Part category
        public int? PartCat { get; set; }

        // bool triggered when reorder value >= On_Hand qty
        public bool IsReorder { get; set; }

        //Model is a specific kind of part (Manufacturer/ Model Number)
        public string ModelName { get; set; }
        // Value of qty needed to trigger reorder
        public int Rdr_Lvl { get; set; }
        // On hand 
        public int On_Hand { get; set; }

    }
}