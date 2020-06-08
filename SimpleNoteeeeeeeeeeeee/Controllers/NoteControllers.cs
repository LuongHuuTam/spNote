using SimpleNoteeeeeeeeeeeee.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNoteeeeeeeeeeeee.Controllers
{
    public class NoteControllers
    {
        public static int getID()
        {
            using (var _context = new SimpleNoteEntities())
            {
                var id = (from n in _context.Notes
                          select n.ID).ToList();
                if (id.Count <= 0)
                    return 1;
                else
                    return id.Max() + 1;
            }
        }
        public static bool addNote(Note note)
        {

            using (var _context = new SimpleNoteEntities())
            {
                foreach (var t in note.Tags)
                {
                    var tag = (from u in _context.Tags
                               where u.Tags == t.Tags
                               select u).Single();
                    tag.Notes.Add(note);
                }
                note.Tags.Clear();
                _context.Notes.AddOrUpdate(note);
                _context.SaveChanges();
                return true;
            }
            
        }
        public static List<Note> getListNote()
        {
            using (var _context = new SimpleNoteEntities())
            {
                var ln = (from u in _context.Notes.AsEnumerable()
                          where u.IsTrash == false
                          select new
                          { 
                              ID=u.ID,
                              Descriptions=u.Descriptions,
                              IsPin=u.IsPin,
                              IsTrash=u.IsTrash,
                              Modified=u.Modified,
                              Tags=u.Tags,
                              Title=u.Title
                          }).Select(x => new Note
                          {
                              ID = x.ID,
                              Descriptions = x.Descriptions,
                              IsPin = x.IsPin,
                              IsTrash = x.IsTrash,
                              Modified = x.Modified,
                              Tags = x.Tags,
                              Title=x.Title
                          });
                return ln.ToList();
            }
        }
        public static Note getNote(string note)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var n = (from u in (from i in _context.Notes.AsEnumerable()
                                    where i.IsTrash == false
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
        public static void deleteNote(Note note)
        {
            using(var _context=new SimpleNoteEntities())
            {
                var notedb = (from n in _context.Notes
                              where n.ID == note.ID
                              select n).SingleOrDefault();
                foreach (Tag i in _context.Tags)
                {
                    foreach(Note m in i.Notes)
                    {
                        if (m.ID==note.ID)
                        {
                            i.Notes.Remove(note);
                            break;
                        }
                    }
                }
                _context.Notes.Remove(notedb);
                _context.SaveChanges();
            }
        }
        public static void updateNote(Note note)
        {
            using (var _context = new SimpleNoteEntities())
            {
                deleteNote(note);
                addNote(note);
            }
        }
        public static List<Note> getListNote(string str)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var ln = (from u in (from i in _context.Notes.AsEnumerable()
                                     where i.IsTrash == false
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
