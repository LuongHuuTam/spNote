using SimpleNoteeeeeeeeeeeee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNoteeeeeeeeeeeee.Controllers
{
    public class TrashControllers
    {
        public static List<Note> getListTrash()
        {
            using (var _context = new SimpleNoteEntities())
            {
                var ln = (from u in _context.Notes.AsEnumerable()
                          where u.IsTrash == true
                          select u).Select(x => new Note
                          {
                              ID = x.ID,
                              Descriptions = x.Descriptions,
                              IsPin = x.IsPin,
                              IsTrash = x.IsTrash,
                              Modified = x.Modified,
                              Tags = x.Tags,
                              Title = x.Title
                          }).ToList();
                return ln;
            }
        }
        public static Note getTrash(string note)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var n = (from u in (from i in _context.Notes.AsEnumerable()
                                    where i.IsTrash == true
                                    select i)
                         where u.Title == note
                         select new
                         {
                             ID = u.ID,
                             Descriptions = u.Descriptions,
                             IsPin = u.IsPin,
                             IsTrash = u.IsTrash,
                             Modified = u.Modified,
                             Tags = u.Tags,
                             Title = u.Title
                         }).Select(x => new Note
                         {
                             ID = x.ID,
                             Descriptions = x.Descriptions,
                             IsPin = x.IsPin,
                             IsTrash = x.IsTrash,
                             Modified = x.Modified,
                             Tags = x.Tags,
                             Title = x.Title
                         }).SingleOrDefault();
                return n;
            }
        }
        public static List<Note> getListTrash(string str)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var ln = (from u in (from i in _context.Notes.AsEnumerable()
                                     where i.IsTrash == true
                                     select i)
                          where u.Title.Contains(str)
                          select new
                          {
                              ID = u.ID,
                              Descriptions = u.Descriptions,
                              IsPin = u.IsPin,
                              IsTrash = u.IsTrash,
                              Modified = u.Modified,
                              Tags = u.Tags,
                              Title = u.Title
                          }).Select(x => new Note
                          {
                              ID = x.ID,
                              Descriptions = x.Descriptions,
                              IsPin = x.IsPin,
                              IsTrash = x.IsTrash,
                              Modified = x.Modified,
                              Tags = x.Tags,
                              Title = x.Title
                          });
                return ln.ToList();
            }
        }
    }
}
