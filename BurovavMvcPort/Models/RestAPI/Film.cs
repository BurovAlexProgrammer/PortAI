using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurovavMvcPort.Models.RestAPI
{
    public class Film : IFilm
    {
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public long id { get; set; }
        public string backdrop_path { get; set; }
        public string original_language { get; set; }
        public string title { get; set; }
        public int[] genre_ids { get; set; }
        DateTime? _release_date { get; set; }
        public DateTime? release_date { get { return _release_date; } set { _release_date = value; } }
        int? _year;
        public int? year { get { return release_date?.Year; } }
        public string overview { get; set; }
    }

    public interface IFilm
    {
        float popularity { get; set; }
        string poster_path { get; set; }
        long id { get; set; }
        string backdrop_path { get; set; }
        string original_language { get; set; }
        string title { get; set; }
        int[] genre_ids { get; set; }
        DateTime? release_date { get; set; }
        int? year { get; }
        string overview { get; set; }
    }
}
