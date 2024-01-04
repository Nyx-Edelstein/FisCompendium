using System;
using System.Collections.Generic;
using FisCompendium.Data.System_Data;
using FisCompendium.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Controllers
{
    public class StoryController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromQuery(Name = "chapter")] string inputChapter)
        {
            string chapter;
            if (string.IsNullOrWhiteSpace(inputChapter))
            {
                chapter = TryGetFromCookie() ?? "A1";
            }
            else
            {
                chapter = inputChapter.ToUpper();
                if (chapter != "END") TrySetCookie(chapter);
            }

            ChapterData chapterData;
            if (ChapterData.Chapters.ContainsKey(chapter))
            {
                chapterData = ChapterData.Chapters[chapter];
            }
            else
            {
                chapterData = new ChapterData
                {
                    Title = "???",
                    ViewName = "???"
                };
            }

            return View(chapterData);
        }

        private const string CURRENT_CHAPTER_COOKIE = "CURRENT_CHAPTER";
        private void TrySetCookie(string chapter)
        {
            if (HttpContext == null) return;

            if (HttpContext.Request.Cookies.ContainsKey(CURRENT_CHAPTER_COOKIE))
                HttpContext.Response.Cookies.Delete(CURRENT_CHAPTER_COOKIE);

            HttpContext.Response.Cookies.Append(CURRENT_CHAPTER_COOKIE, chapter, new CookieOptions
            {
                Expires = DateTimeOffset.MaxValue
            });
        }

        private string TryGetFromCookie()
        {
            if (HttpContext == null) return null;

            if (!HttpContext.Request.Cookies.ContainsKey(CURRENT_CHAPTER_COOKIE)) return null;

            var chapter = HttpContext.Request.Cookies[CURRENT_CHAPTER_COOKIE];
            return chapter;
        }
    }

    public class StoryData
    {
        private IRepository<ConfigItem> ConfigItemRepository { get; }

        public StoryData() : this(RepositoryFactory.Create<ConfigItem>(DatabaseSelector.System)) { }

        public StoryData(IRepository<ConfigItem> _configItemRepository)
        {
            ConfigItemRepository = _configItemRepository;
        }

        public static string CURRENT_CHAPTER = "C34";
        public static string LAST_PUBLISHED_DATE = "April 10, 2023";
        public static string DISCORD_LINK = @"https://discord.gg/zVaWgxVX4B";
        public static string SV_LINK = @"https://forums.sufficientvelocity.com/threads/legacy-of-the-goddess-a-rational-zelda-quest.72138/";
    }

    public class ChapterData
    {
        public string Title { get; set; }
        public string ViewName { get; set; }
        public string Start { get; set; } = "TOC";
        public string Prev { get; set; }
        public string Next { get; set; }
        public string Current { get; set; } = StoryData.CURRENT_CHAPTER;
        public bool HasPlan { get; set; }

        public static Dictionary<string, ChapterData> Chapters = new Dictionary<string, ChapterData>
        {
            {
                "TOC", new ChapterData
                {
                    Title = "Table of Contents",
                    ViewName = "__TOC",
                    Start = null,
                    Prev = null,
                    Next = "C1",
                }
            },
            {
                "END", new ChapterData
                {
                    Title = "Progress Point",
                    ViewName = "__END",
                    Prev = StoryData.CURRENT_CHAPTER,
                    Next = null,
                    Current = null
                }
            },
            {
                "A1", new ChapterData
                {
                    Title = "Prologue (1/2)",
                    ViewName = "_A1",
                    Prev = "C1",
                    Next = "A2",
                }
            },
            {
                "A2", new ChapterData
                {
                    Title = "Prologue (2/2)",
                    ViewName = "_A2",
                    Prev = "A1",
                    Next = "C2",
                }
            },
            {
                "C1", new ChapterData
                {
                    Title = "Chapter 1: A Blight Upon the Realm",
                    ViewName = "_C0001",
                    Prev = "TOC",
                    Next = "A1",
                }
            },
            {
                "C2", new ChapterData
                {
                    Title = "Chapter 2: Reversal of Fate",
                    HasPlan = true,
                    ViewName = "_C0002",
                    Prev = "C1",
                    Next = "C3",
                }
            },
            {
                "C3", new ChapterData
                {
                    Title = "Chapter 3: Guidance and Direction",
                    HasPlan = true,
                    ViewName = "_C0003",
                    Prev = "C2",
                    Next = "C4",
                }
            },
            {
                "C4", new ChapterData
                {
                    Title = "Chapter 4: To Be Queen",
                    HasPlan = true,
                    ViewName = "_C0004",
                    Prev = "C3",
                    Next = "C5",
                }
            },
            {
                "C5", new ChapterData
                {
                    Title = "Chapter 5: Revelations",
                    HasPlan = true,
                    ViewName = "_C0005",
                    Prev = "C4",
                    Next = "C6.1",
                }
            },
            {
                "C6.1", new ChapterData
                {
                    Title = "Chapter 6.1: Lost",
                    HasPlan = true,
                    ViewName = "_C0006.1",
                    Prev = "C5",
                    Next = "C6.2",
                }
            },
            {
                "C6.2", new ChapterData
                {
                    Title = "Chapter 6.2: But Not Forgotten",
                    ViewName = "_C0006.2",
                    Prev = "C6.1",
                    Next = "C7",
                }
            },
            {
                "C7", new ChapterData
                {
                    Title = "Chapter 7: In Search of Answers",
                    HasPlan = true,
                    ViewName = "_C0007",
                    Prev = "C6.2",
                    Next = "C8",
                }
            },
            {
                "C8", new ChapterData
                {
                    Title = "Chapter 8: At the Temple of Time",
                    HasPlan = true,
                    ViewName = "_C0008",
                    Prev = "C7",
                    Next = "C9",
                }
            },
            {
                "C9", new ChapterData
                {
                    Title = "Chapter 9: Link’s Awakening",
                    HasPlan = true,
                    ViewName = "_C0009",
                    Prev = "C8",
                    Next = "C10",
                }
            },
            {
                "F9", new ChapterData
                {
                    Title = "Chapter 9: Link’s Awakening (April Fools’ 2020)",
                    ViewName = "_C0009F",
                    Prev = "C8",
                    Next = "C10",
                }
            },
            {
                "C10", new ChapterData
                {
                    Title = "Chapter 10: First of the Sunseekers",
                    HasPlan = true,
                    ViewName = "_C0010",
                    Prev = "C9",
                    Next = "C11",
                }
            },
            {
                "C11", new ChapterData
                {
                    Title = "Chapter 11: Sands and Sun",
                    HasPlan = true,
                    ViewName = "_C0011",
                    Prev = "C10",
                    Next = "C12",
                }
            },
            {
                "C12", new ChapterData
                {
                    Title = "Chapter 12: Dreams of the Damned",
                    HasPlan = true,
                    ViewName = "_C0012",
                    Prev = "C11",
                    Next = "C13",
                }
            },
            {
                "C13", new ChapterData
                {
                    Title = "Chapter 13: The Face of the Enemy",
                    HasPlan = true,
                    ViewName = "_C0013",
                    Prev = "C12",
                    Next = "C14",
                }
            },
            {
                "C14", new ChapterData
                {
                    Title = "Chapter 14: Inquiring Minds",
                    HasPlan = true,
                    ViewName = "_C0014",
                    Prev = "C13",
                    Next = "C15",
                }
            },
            {
                "C15", new ChapterData
                {
                    Title = "Chapter 15: Catharsis",
                    ViewName = "_C0015",
                    Prev = "C14",
                    Next = "C16",
                }
            },
            {
                "C16", new ChapterData
                {
                    Title = "Chapter 16: The Obsidian Spire (1/3)",
                    HasPlan = true,
                    ViewName = "_C0016",
                    Prev = "C15",
                    Next = "C17",
                }
            },
            {
                "C17", new ChapterData
                {
                    Title = "Chapter 17: The Obsidian Spire (2/3)",
                    HasPlan = true,
                    ViewName = "_C0017",
                    Prev = "C16",
                    Next = "C18",
                }
            },
            {
                "C18", new ChapterData
                {
                    Title = "Chapter 18: The Obsidian Spire (3/3)",
                    HasPlan = true,
                    ViewName = "_C0018",
                    Prev = "C17",
                    Next = "C19",
                }
            },
            {
                "C19", new ChapterData
                {
                    Title = "Chapter 19: Loose Ends",
                    HasPlan = true,
                    ViewName = "_C0019",
                    Prev = "C18",
                    Next = "C20",
                }
            },
            {
                "C20", new ChapterData
                {
                    Title = "Chapter 20: Entitled Opinions",
                    HasPlan = true,
                    ViewName = "_C0020",
                    Prev = "C19",
                    Next = "C21",
                }
            },
            {
                "C21", new ChapterData
                {
                    Title = "Chapter 21: Patience, and Other Lost Arts",
                    HasPlan = true,
                    ViewName = "_C0021",
                    Prev = "C20",
                    Next = "C22.1",
                }
            },
            {
                "C22.1", new ChapterData
                {
                    Title = "Chapter 22.1: Ibboso the Bold",
                    HasPlan = true,
                    ViewName = "_C0022.1",
                    Prev = "C21",
                    Next = "C22.2",
                }
            },
            {
                "C22.2", new ChapterData
                {
                    Title = "Chapter 22.2: Ibboso the Damned",
                    HasPlan = true,
                    ViewName = "_C0022.2",
                    Prev = "C22.1",
                    Next = "C23",
                }
            },
            {
                "C23", new ChapterData
                {
                    Title = "Chapter 23: To Buy a Kingdom",
                    HasPlan = true,
                    ViewName = "_C0023",
                    Prev = "C22.2",
                    Next = "C24",
                }
            },
            {
                "C24", new ChapterData
                {
                    Title = "Chapter 24: Fortune Finds the Bold",
                    ViewName = "_C0024",
                    Prev = "C23",
                    Next = "C25",
                }
            },
            {
                "C25", new ChapterData
                {
                    Title = "Chapter 25: Rendezvous",
                    HasPlan = true,
                    ViewName = "_C0025",
                    Prev = "C24",
                    Next = "C26",
                }
            },
            {
                "C26", new ChapterData
                {
                    Title = "Chapter 26: Cataclysm’s Eve",
                    HasPlan = false,
                    ViewName = "_C0026",
                    Prev = "C25",
                    Next = "C27",
                }
            },
            {
                "C27", new ChapterData
                {
                    Title = "Chapter 27: Gugwayah",
                    HasPlan = true,
                    ViewName = "_C0027",
                    Prev = "C26",
                    Next = "C28",
                }
            },
            {
                "C28", new ChapterData
                {
                    Title = "Chapter 28: Mending Wounds",
                    HasPlan = true,
                    ViewName = "_C0028",
                    Prev = "C27",
                    Next = "C29",
                }
            },
            {
                "C29", new ChapterData
                {
                    Title = "Chapter 29: Finding Wisdom",
                    HasPlan = true,
                    ViewName = "_C0029",
                    Prev = "C28",
                    Next = "C30",
                }
            },
            {
                "C30", new ChapterData
                {
                    Title = "Chapter 30: Pondering Reflections",
                    HasPlan = true,
                    ViewName = "_C0030",
                    Prev = "C29",
                    Next = "C31",
                }
            },
            {
                "C31", new ChapterData
                {
                    Title = "Chapter 31: The Mind’s Eye",
                    HasPlan = true,
                    ViewName = "_C0031",
                    Prev = "C30",
                    Next = "C32",
                }
            },
            {
                "C32", new ChapterData
                {
                    Title = "Chapter 32: Confrontations",
                    HasPlan = true,
                    ViewName = "_C0032",
                    Prev = "C31",
                    Next = "C33",
                }
            },
            {
                "C33", new ChapterData
                {
                    Title = "Chapter 33: Betrayal and Trust",
                    HasPlan = false,
                    ViewName = "_C0033",
                    Prev = "C32",
                    Next = "C34",
                }
            },
            {
                "C34", new ChapterData
                {
                    Title = "Chapter 34: The Ruined Palace",
                    HasPlan = true,
                    ViewName = "_C0034",
                    Prev = "C33",
                    //Next = "C35",
                    Next = "END",
                    Current = "END",
                }
            },
        };
    }
}
