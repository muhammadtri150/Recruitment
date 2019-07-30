using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;
using FinalProject.DTO;

namespace FinalProject.DTO
{
    public class ZipCodeDTO
    {
        public int id { get; set; }
        public string kelurahan { get; set; }
        public string kecamatan { get; set; }
        public string kabupaten { get; set; }
        public string provinsi { get; set; }
        public string kodepos { get; set; }
    }

    public class Manage_ZipCodeDTO
    {
        public static ZipCodeDTO GetData(object code)
        {
            string ZipCode = code.ToString();

            using (DBEntities db = new DBEntities())
            {
                return (ZipCodeDTO)db.TB_ZIPCODE.Select(z => new ZipCodeDTO
                {
                    id = z.id,
                    kelurahan = z.kelurahan,
                    kecamatan = z.kecamatan,
                    kabupaten = z.kabupaten,
                    provinsi = z.provinsi,
                    kodepos = z.kodepos
                }).FirstOrDefault(z => z.kodepos == ZipCode);
            }
        }
    }
}