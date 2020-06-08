using SimpleNoteeeeeeeeeeeee.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNoteeeeeeeeeeeee.Controllers
{
    public class TagControllers
    {
        public static void addTag(Tag tag)
        {
            using (var _context = new SimpleNoteEntities())
            {
                _context.Tags.AddOrUpdate(tag);
                _context.SaveChanges();
            }
        }
        public static List<Note> getListNote(string str)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var t = (from i in _context.Tags.AsEnumerable()
                         where i.Tags.Contains(str)
                         select new
                         {
                             tags = i.Tags,
                             notes = i.Notes
                         }).Select(x => new Tag
                         {
                             Tags = x.tags,
                             Notes = x.notes
                         }).ToList();
                List<Note> ln = new List<Note>();
                foreach (var m in t)
                {
                    foreach (Note note in m.Notes)
                    {
                        if (note.IsTrash == false)
                        {
                            foreach (Note temp in ln)
                            {
                                if (temp.Title == note.Title)
                                {
                                    ln.Remove(temp);
                                    break;
                                }
                            }
                            ln.Add(note);
                        }
                    }
                }
                return ln;
            }
        }
        public static List<Note> getListTrash(string str)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var t = (from i in _context.Tags.AsEnumerable()
                         where i.Tags.Contains(str)
                         select new
                         {
                             tags = i.Tags,
                             notes = i.Notes
                         }).Select(x => new Tag
                         {
                             Tags = x.tags,
                             Notes = x.notes
                         }).ToList();
                List<Note> ln = new List<Note>();
                foreach (var m in t)
                {
                    foreach (Note note in m.Notes)
                    {
                        if (note.IsTrash == true)
                        {
                            foreach (Note temp in ln)
                            {
                                if (temp.Title == note.Title)
                                {
                                    ln.Remove(temp);
                                    break;
                                }
                            }
                            ln.Add(note);
                        }
                    }
                }
                return ln;
            }
        }
        public static List<Tag> getListTag()
        {
            using (var _context = new SimpleNoteEntities())
            {
                var lt = (from t in _context.Tags.AsEnumerable()
                          select new
                          {
                              tag = t.Tags,
                              notes = t.Notes
                          }).Select(x => new Tag { Tags = x.tag, Notes = x.notes }).ToList();
                return lt;
            }
        }
        public static void deleteTag(Tag tag)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var dbtag = (from t in _context.Tags
                             where t.Tags == tag.Tags
                             select t).Single();
                foreach(var i in _context.Notes)
                {
                    foreach(var n in i.Tags)
                    {
                        if (n.Tags==tag.Tags)
                        {
                            i.Tags.Remove(tag);
                            break;
                        }
                    }
                }
                _context.Tags.Remove(dbtag);
                _context.SaveChanges();
            }
        }
        public static Tag getTag(string str)
        {
            using (var _context = new SimpleNoteEntities())
            {
                var t = (from u in _context.Tags.AsEnumerable()
                         where u.Tags == str
                         select new { tag = u.Tags, note = u.Notes }).Select(x => new Tag { Tags = x.tag, Notes = x.note }).Single();
                return t;
            }
        }
    }
}
