/* This file contains classes to wrap types from answers.xml file [Volodya Shtenyovych] 
 */

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.Course
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Serialization;

    /// <summary>
    /// This class represent storage of answers
    /// </summary>
    [Serializable]
    [XmlRoot("answers")]
    [XmlInclude(typeof(Organization))]
    [XmlInclude(typeof(Item))]
    [XmlInclude(typeof(Question))]
    [XmlInclude(typeof(CompiledQuestion))]
    public class Answers
    {
        [NotNull]
        public static Answers FromFile([NotNull]string fileName)
        {
#if CHECKERS
            if (!File.Exists(fileName))
            {
                throw new FireFlyException("File '{0}' is not exists", fileName);
            }
#endif
            using (var s = File.OpenRead(fileName))
            {
                return (Answers)Serializer.Deserialize(s);
            }
        }

        /// <summary>
        /// Serializer to [de]serialize Answer class from/to xml.
        /// </summary>
        [NotNull]
        public static XmlSerializer Serializer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private OrganizationCollection _orgs;

        public void SaveToFile([NotNull]string fileName)
        {
            using (var s = File.Create(fileName))
            {
                Serializer.Serialize(s, this);
            }
        }

        /// <summary>
        /// Gets an array of organizations contain in storage
        /// </summary>
        [XmlElement("organization")]
        [NotNull]
        public OrganizationCollection Organizations
        {
            get
            {
                if (_orgs == null)
                {
                    _orgs = new OrganizationCollection();
                }
                return _orgs;
            }
            set { _orgs = value; }
        }

        /// <summary>
        /// Updates answers using specified information
        /// </summary>
        /// <param name="itemId">Id of item should be updated</param>
        /// <param name="passRank">PassRank should be assigned for this page</param>
        /// <param name="questions">Answers for questions should be assigned for this page</param>
        /// <returns>Updated answers storage</returns>
        public void Update([NotNull]string itemId, int? passRank, [NotNull]IEnumerable<Question> questions)
        {
            Item i = Organizations[Course.Organization.identifier].Items[itemId];
            i.PassRank = passRank;
            i.Questions.Clear();
            i.Questions.AddRange(questions);
        }

        public void RemoveItem([NotNull]string itemID)
        {
            if (Organizations[Course.Organization.identifier] != null)
            {
#if CHECKERS
                var removed =
#endif
 Organizations[Course.Organization.identifier].Items.RemoveAll(i => i.Id == itemID);
#if CHECKERS
                if (removed != 1)
                {
                    throw new FireFlyException("Incorrect count of items has been removed {0}", removed);
                }
#endif
            }
        }
        public void RenameItem([NotNull]string itemId, [NotNull]string newItemId)
        {
            var org = Organizations[Course.Organization.identifier];
            var items = org.Items;
            if (items == null)
            {
                return;
            }
            var item = items.Find(i => i.Id == itemId);
            if (item == null)
            {
                return;
            }
            item.Id = newItemId;
        }

        #region Nested type: OrganizationCollection

        /// <summary>
        /// Represents collection of organization classes
        /// </summary>
        public class OrganizationCollection : List<Organization>
        {
            /// <summary>
            /// Gets organization from collection by it's id.
            /// </summary>
            /// <param name="id">Id for organization should be extracted</param>
            /// <returns>Organization with specified id.</returns>
            public Organization this[string id]
            {
                get
                {
                    var res = Find(o => o.Id == id);
                    if (res == null)
                    {
                        Add(res = new Organization(id));
                    }
                    return res;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a storage of answers for single organization.
    /// </summary>
    [Serializable]
    public class Organization
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _id;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ItemCollection _items;

        /// <summary>
        /// Creates new instance of organization using default parameters.
        /// </summary>
        public Organization()
        {
        }

        /// <summary>
        /// Creates new instance with specified ID.
        /// </summary>
        /// <param name="id">ID should be assigned for new instance</param>
        public Organization([NotNull]string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets collection of answers for item
        /// </summary>
        [XmlElement("item")]
        [NotNull]
        public ItemCollection Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ItemCollection();
                }
                return _items;
            }
            set { _items = value; }
        }

        /// <summary>
        /// Gets or sets Id of organization
        /// </summary>
        [XmlAttribute("id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        #region Nested type: ItemCollection

        /// <summary>
        /// Represents collection of answers for items
        /// </summary>
        public class ItemCollection : List<Item>
        {
            /// <summary>
            /// Gets Item by id.
            /// </summary>
            /// <param name="id">Id of items should be extracted</param>
            /// <returns>Item with specified id or null if it doesn't contains in collection</returns>
            public Item this[string id]
            {
                get
                {
                    var res = Find(i => i.Id == id);
                    if (res == null)
                    {
                        throw new FireFlyException("Record for '{0}' was not found", id);
                    }
                    return res;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents answer for single item
    /// </summary>
    [Serializable]
    public class Item
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Question> _questions;

        /// <summary>
        /// Creates new instance of answers for item with default parameters
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        /// Creates new instance of answers fo item with specified ID.
        /// </summary>
        /// <param name="id">Id should be assigned to object</param>
        public Item(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets Answers for questions of item
        /// </summary>
        [XmlElement("question")]
        [NotNull]
        public List<Question> Questions
        {
            get
            {
                if (_questions == null)
                {
                    _questions = new List<Question>();
                }
                return _questions;
            }
            set { _questions = value; }
        }

        /// <summary>
        /// Gets or sets id of item
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets PassRank for item
        /// </summary>

        [XmlElement("passRank")]
        public int? PassRank { get; set; }
    }

    /// <summary>
    /// Represents answer of single question
    /// </summary>
    [Serializable]
    public class Question
    {
        /// <summary>
        /// Creates new instance of Question class with default parameters
        /// </summary>
        public Question()
        {
        }

        /// <summary>
        /// Creates new instance of Quesiton class with specified information
        /// </summary>
        /// <param name="answer">Answer for question</param>
        /// <param name="rank">Rank of question</param>
        public Question([NotNull]string answer, int? rank)
        {
            Answer = answer;
            Rank = rank;
        }

        /// <summary>
        /// Gets or sets answer of question
        /// </summary>
        [XmlAttribute("answer")]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets rank of question
        /// </summary>
        [XmlElement("rank")]
        public int? Rank { get; set; }
    }

    public class CompiledQuestion : Question
    {
        public enum LANGUAGE
        {
            CS,
            CPP,
            Delphi,
            Java
        }

        public CompiledQuestion()
        {
        }


        public static string GetLanguageString(LANGUAGE? lang)
        {
            if (lang == null)
            {
                return "";
            }
            return System.Enum.GetName(typeof(LANGUAGE), lang);
        }

        public CompiledQuestion(int? rank, long memoryLimit, long outputLimit, long timeLimit, LANGUAGE? language)
            : base(null, rank)
        {
            MemoryLimit = memoryLimit;
            OutputLimit = outputLimit;
            TimeLimit = timeLimit;
            Language = language;
        }

        [XmlAttribute("memory_limit")]
        public long MemoryLimit;

        [XmlAttribute("time_limit")]
        public long TimeLimit;

        [XmlAttribute("output_limit")]
        public long OutputLimit;

        [XmlElement("language")]
        public LANGUAGE? Language;

        [XmlElement("testcase")]
        [NotNull]
        public List<CompiledTestCase> Tests
        {
            get
            {
                if (_Tests == null)
                {
                    _Tests = new List<CompiledTestCase>();
                }
                return _Tests;
            }
            set { _Tests = value; }
        }

        private List<CompiledTestCase> _Tests;
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CompiledTestCase
    {
        public CompiledTestCase()
        {
        }

        public CompiledTestCase([CanBeNull]string input, [CanBeNull]string output)
        {
            Input = input;
            Output = output;
        }

        [XmlElement("output")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [CanBeNull]
        public string Output { get; set; }

        [XmlElement("input")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [CanBeNull]
        public string Input { get; set; }
    }
}