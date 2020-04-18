using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurovavMvcPort.Models
{
    public class Genre : IGenre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    //Жанр фильма
    public interface IGenre
    {
        int Id { get; set; } //ID элемента из TMDB
        string Name { get; set; } //Название жанра
    }

    //Класс десериализации
    public class Genres
    {
        public List<Genre> genres;
    }
}
