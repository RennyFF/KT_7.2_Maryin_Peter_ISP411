//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MasterFloorApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Adresses
    {
        public int Id { get; set; }
        public int PostCodeId { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public int StreetId { get; set; }
        public string HouseNumber { get; set; }
    
        public virtual Cities Cities { get; set; }
        public virtual PostCodes PostCodes { get; set; }
        public virtual Regions Regions { get; set; }
        public virtual Streets Streets { get; set; }
    }
}
