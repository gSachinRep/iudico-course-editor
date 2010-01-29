using FireFly.CourseEditor.Course;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System.ComponentModel;
    using System.Diagnostics;

    ///<summary>
    /// Base class for all Exam controls in Editor
    ///</summary>
    public abstract class HtmlTestControl : HtmlDesignMovableControl
    {
        private const string RANK_PROPERTY = "Rank",
                 RANK_PROPERTY_ERROR = "'" + RANK_PROPERTY + "' must be specified",
                 RANK_PROPERTY_ERROR2 = "'" + RANK_PROPERTY + "' must be a positive number";

        ///<summary>
        /// Count of points will be given to user if answer typed correctly
        ///</summary>
        [Category("Data")]
        [DisplayName(RANK_PROPERTY)]
        [Description("Count of points will be given to user if answer typed correctly")]
        public int? Rank
        {
            get { return _Rank; }
            set
            {
                if (_Rank != value)
                {
                    if (_Rank == null && value != null)
                    {
                        RemoveError(RANK_PROPERTY);
                    }
                    _Rank = value;               
                    if (value == null)
                    {
                        AddError(RANK_PROPERTY, RANK_PROPERTY_ERROR);
                    }
                    else
                    {
                        if (value <= 0)
                        {
                            AddError(RANK_PROPERTY_ERROR2);
                        }
                    }
                }
            }
        }

        ///<summary>
        /// String representation of answer that should be accepted as correct
        ///</summary>
        [Browsable(false)]
        public abstract string CorrectAnswer { get; set; }

        ///<summary>
        /// Gets JavaScript code to initialize SCO object for this Exam class 
        ///</summary>
        ///<returns></returns>
        public abstract string GetScoTestInitializer();

        protected override void InternalValidate()
        {
            base.InternalValidate();
            if (_Rank == null)
            {
                AddError(RANK_PROPERTY, RANK_PROPERTY_ERROR);
            } else if (_Rank <= 0)
            {
                AddError(RANK_PROPERTY_ERROR2);
            }
        }

        ///<summary>
        /// Updates state of Exam control based on item from answer file
        ///</summary>
        ///<param name="q"></param>
        public virtual void ReadAnswerItem(Question q)
        {
            Rank = q.Rank;
            CorrectAnswer = q.Answer;
        }

        ///<summary>
        /// Create item should be stored in answer file
        ///</summary>
        ///<returns></returns>
        public virtual Question StoreAnswersItem()
        {
            return new Question(CorrectAnswer, Rank);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? _Rank = 1;
    }
}