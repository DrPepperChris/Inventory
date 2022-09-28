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
        /// <summary>
        /// The model id of individual item
        /// </summary>
        /// <example>UUID of item in warehouse</example>
        [Required]
        [Key]
        public int ModelId { get; set; }
        [Required]
        public int WarehouseId { get; set; }

        public int StoreId { get; set; }

        //NMN
        public string Nomenclature { get; set; }

        //Part category
        public int? PartCat { get; set; }

        // bool triggered when reorder value >= On_Hand qty
        public bool IsReorder { get; set; }

        //Model is a specific kind of part (Manufacturer/ Model Number)
        public string ModelName { get; set; }

        public int Rdr_Lvl { get; set; }

        public int On_Hand { get; set; }

    }
}