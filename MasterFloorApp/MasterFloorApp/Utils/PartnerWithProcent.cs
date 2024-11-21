using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterFloorApp.Utils
{
    public class PartnerWithProcent : Model.Partners
    {
        public int SaleProcent { get; set; }
        public PartnerWithProcent(Model.Partners Partner)
        {
            this.Id = Partner.Id;
            this.PartnerType = Partner.PartnerType;
            this.Name = Partner.Name;
            this.Directors = Partner.Directors;
            this.Phone = Partner.Phone;
            this.Rating = Partner.Rating;

            var PartnerProducts = Model.MasterFloorDBEntities.GetContext().PartnerProducts
                .Any(i => i.PartnerId == Partner.Id);
            if (PartnerProducts)
            {
                int RealisationSum = Model.MasterFloorDBEntities.GetContext().PartnerProducts
                    .Where(i => i.PartnerId == Partner.Id)
                    .Sum(i=> i.Countity);
                this.SaleProcent = RealisationSum < 10000 ? 0 : 
                    RealisationSum>=10000 && RealisationSum<50000 ? 5 : 
                    RealisationSum>=50000 && RealisationSum < 300000 ? 10 : 
                    15;
            }
            else
            {
                this.SaleProcent = 0;
            }
        }
    }
}
