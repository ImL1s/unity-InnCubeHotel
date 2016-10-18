using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UI.Tables;

namespace UI.Dates
{
    [ExecuteInEditMode]
    public class DatePicker : MonoBehaviour
    {
        #region Dates
        [SerializeField]
        private SerializableDate m_SelectedDate;
        public SerializableDate SelectedDate 
        { 
            get { return m_SelectedDate; } 
            set 
            {                
                SetProperty(ref m_SelectedDate, value);
                
                // This will update the VisibleDate field to match the selected date, ensuring that the currently visible month always matches the selected date
                // (when the date is selected)
                // This is only relevant when selecting dates that don't fall within the current month
                if (Config.Misc.SwitchToSelectedMonthWhenDateSelected)
                {
                    VisibleDate = value;
                }

                if (Ref_InputField != null)
                {                    
                    Ref_InputField.text = (SelectedDate.HasValue) ? SelectedDate.Date.ToString(Config.Format.DateFormat) : "";
                }

                if (Config.Misc.CloseWhenDateSelected) Hide();
            } 
        }

        [SerializeField]
        private SerializableDate m_VisibleDate;        
        public SerializableDate VisibleDate 
        { 
            get 
            {
                if (!m_VisibleDate.HasValue)
                {
                    if (SelectedDate.HasValue)
                    {                        
                        m_VisibleDate = new SerializableDate(SelectedDate.Date);
                    }
                    else
                    {                     
                        m_VisibleDate = new SerializableDate(DateTime.Today);
                    }
                }

                return m_VisibleDate; 
            } 
            set { SetProperty(ref m_VisibleDate, value); } 
        }
        #endregion

        #region Config
        public DatePickerConfig Config;                
        #endregion

        #region References
        [Header("References")]
        public RectTransform Ref_DatePickerTransform;
        public DatePicker_Header Ref_Header;

        public TableLayout Ref_DayTable;
        public DatePicker_Animator Ref_DayTableAnimator;
        public TableCell Ref_DayTableContainer;
        
        public DatePicker_DayHeader Ref_Template_DayName;
        public DatePicker_DayButton Ref_Template_Day_CurrentMonth;
        public DatePicker_DayButton Ref_Template_Day_OtherMonths;
        public DatePicker_DayButton Ref_Template_Day_Today;
        public DatePicker_DayButton Ref_Template_Day_SelectedDay;

        public Image Ref_Border;
        public DatePicker_ContentLayout Ref_ContentLayout;

        public Image Ref_ScreenOverlay;
        public DatePicker_Animator Ref_ScreenOverlayAnimator;

        public DatePicker_Animator Ref_Animator;

        // Optional
        public InputField Ref_InputField;
        public TableLayout Ref_InputFieldContainer;
        public TableCell Ref_InputFieldToggleButtonCell;

        public DatePicker_DateRange Ref_DatePicker_DateRange;
        #endregion

        /// <summary>
        /// Only relevant for popup datepickers
        /// </summary>
        private DatePickerPosition calculatedPosition = DatePickerPosition.Below;

        public bool IsVisible
        {
            get
            {
                return Ref_DatePickerTransform.gameObject.activeInHierarchy;
            }
        }
        
        void Start()
        {            
            UpdateDisplay();                        
        }

        public void ShowPreviousMonth()
        {
            VisibleDate = VisibleDate.Date.AddMonths(-1);
            MonthChangedUpdateDisplay();  
        }

        public void ShowNextMonth()
        {            
            VisibleDate = VisibleDate.Date.AddMonths(1);
            MonthChangedUpdateDisplay();                       
        }

        public void ShowPreviousYear()
        {
            VisibleDate = VisibleDate.Date.AddYears(-1);
            MonthChangedUpdateDisplay();
        }

        public void ShowNextYear()
        {
            VisibleDate = VisibleDate.Date.AddYears(1);
            MonthChangedUpdateDisplay();
        }

        void MonthChangedUpdateDisplay()
        {
            if (Config.Animation.MonthChangedAnimation == Animation.None)
            {
                UpdateDisplay();
                return;
            }

            Ref_DayTableAnimator.PlayAnimation(Config.Animation.MonthChangedAnimation,
                                               AnimationType.Hide,
                                               () =>
                                               {
                                                   UpdateDisplay();

                                                   Ref_DayTableAnimator.PlayAnimation(Config.Animation.MonthChangedAnimation, AnimationType.Show);
                                               }); 
        }

        public void UpdateDisplay()
        {
            // don't do anything if we aren't actually active in the hierarchy
            // (basically, we're either inactive or a prefab)
            if (!this.gameObject.activeInHierarchy) return;

            if (Config.Sizing.OverrideTransformHeight)
            {
                Ref_DatePickerTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Config.Sizing.PreferredHeight);
            }

            UpdateBorder();
            UpdateHeader();
            UpdateWeekDayHeaders();
            UpdateDaySection();

            // Clear existing data
            Ref_DayTable.ClearRows();

            // Day Names
            var dayNameRow = Ref_DayTable.AddRow(0);
            dayNameRow.dontUseTableRowBackground = true;            

            var dayNames = DatePickerUtilities.GetAbbreviatedDayNames();
            foreach (var dayName in dayNames)
            {
                var header = Instantiate(Ref_Template_DayName);
                dayNameRow.AddCell(header.Cell);

                header.HeaderText.text = dayName;
            }

            // Validate our Date Range (if necessary. This method will output an error message if we fail)
            bool dateRangeValid = Config.DateRange.Validate();

            // Dates
            var days = DatePickerUtilities.GetDateRangeForDisplay(VisibleDate.Date);
            TableRow row = null;
            foreach (var day in days)
            {
                if (day.DayOfWeek == DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                {
                    row = Ref_DayTable.AddRow(0);
                }

                if (!Config.Misc.ShowDatesInOtherMonths && !DatePickerUtilities.DateFallsWithinMonth(day, VisibleDate))
                {
                    // add an empty cell
                    row.AddCell();
                }
                else
                {
                    var dayItem = Instantiate(GetDayTemplateForDate(day));
                    row.AddCell(dayItem.Cell);

                    dayItem.Text.text = day.Day.ToString();
                    dayItem.DatePicker = this;
                    dayItem.Date = day;
                    dayItem.name = day.ToDateString();
                    dayItem.IsTemplate = false;

                    if (dateRangeValid) // if the date range is not valid, then don't attempt to use it
                    {
                        if ((Config.DateRange.RestrictFromDate && day.CompareTo(Config.DateRange.FromDate) < 0)
                         || (Config.DateRange.RestrictToDate && day.CompareTo(Config.DateRange.ToDate) > 0))
                        {
                            dayItem.Button.interactable = false;
                        }
                    }
                }
            }
            
            if (Ref_InputField != null)
            {
                Ref_InputField.text = SelectedDate.HasValue ? SelectedDate.Date.ToString(Config.Format.DateFormat) : "";
                if(Ref_ScreenOverlay != null) Ref_ScreenOverlay.color = Config.Modal.ScreenOverlayColor;
                
                if (Config.InputField.ShowToggleButton)
                {
                    Ref_InputFieldContainer.ColumnWidths = new List<float> { 0, Config.InputField.ToggleButtonWidth };
                    Ref_InputFieldToggleButtonCell.gameObject.SetActive(true);
                }
                else
                {
                    Ref_InputFieldContainer.ColumnWidths = new List<float> { 0 };
                    Ref_InputFieldToggleButtonCell.gameObject.SetActive(false);
                }
            }
        }

        void UpdateBorder()
        {
            // Border size / color
            Ref_ContentLayout.SetBorderSize(Config.Border.Size);
            Ref_Border.color = Config.Border.Color;
        }

        void UpdateHeader()
        {
            // Update month name
            Ref_Header.HeaderText.text = VisibleDate.Date.ToString("MMM yyyy");

            Config.Header.Apply(Ref_Header);

            var dateRangeValid = Config.DateRange.Validate(true);

            if (dateRangeValid && Config.DateRange.RestrictFromDate)
            {
                var lastDayOfPreviousMonth = VisibleDate.Date.AddMonths(-1);
                lastDayOfPreviousMonth = new DateTime(lastDayOfPreviousMonth.Year, lastDayOfPreviousMonth.Month, DateTime.DaysInMonth(lastDayOfPreviousMonth.Year, lastDayOfPreviousMonth.Month)).AddDays(1).AddTicks(-1);

                Ref_Header.PreviousMonthButton.Button.interactable = (lastDayOfPreviousMonth.CompareTo(Config.DateRange.FromDate) >= 0);
            }
            else
            {
                Ref_Header.PreviousMonthButton.Button.interactable = true;
            }

            if (dateRangeValid && Config.DateRange.RestrictToDate)
            {
                var firstDayOfNextMonth = VisibleDate.Date.AddMonths(1);
                firstDayOfNextMonth = new DateTime(firstDayOfNextMonth.Year, firstDayOfNextMonth.Month, 1);                                

                Ref_Header.NextMonthButton.Button.interactable = (firstDayOfNextMonth.CompareTo(Config.DateRange.ToDate) <= 0);
            }
            else
            {
                Ref_Header.NextMonthButton.Button.interactable = true;
            }
        }

        void UpdateWeekDayHeaders()
        {
            Config.WeekDays.ApplyConfig(Ref_Template_DayName);
        }

        void UpdateDaySection()
        {
            var templateList = new List<DatePicker_Button>() 
            { 
                Ref_Template_Day_Today,
                Ref_Template_Day_SelectedDay,
                Ref_Template_Day_CurrentMonth,
                Ref_Template_Day_OtherMonths
            };

            foreach (var template in templateList)
            {
                template.Text.font = Config.Days.Font;
            }

            Config.Days.Today.ApplyConfig(Ref_Template_Day_Today);
            Config.Days.SelectedDay.ApplyConfig(Ref_Template_Day_SelectedDay);
            Config.Days.OtherMonths.ApplyConfig(Ref_Template_Day_OtherMonths);
            Config.Days.CurrentMonth.ApplyConfig(Ref_Template_Day_CurrentMonth);

            Ref_DayTable.RowBackgroundColor = Config.Days.BackgroundColor;            
            Ref_DayTableContainer.image.color = Config.Days.BackgroundColor;
        }

        private DatePicker_DayButton GetDayTemplateForDate(DateTime date)
        {
            DatePicker_DayButton dayTemplate = null;

            if (SelectedDate.HasValue && date.Equals(SelectedDate.Date))
            {
                dayTemplate = Ref_Template_Day_SelectedDay;
            }
            else if (date.Equals(DateTime.Today))
            {
                dayTemplate = Ref_Template_Day_Today;
            }
            else if (date.Month == VisibleDate.Date.Month)
            {
                dayTemplate = Ref_Template_Day_CurrentMonth;
            }
            else
            {
                dayTemplate = Ref_Template_Day_OtherMonths;
            }

            return dayTemplate;
        }

        /// <summary>
        /// Called by DayButton
        /// </summary>
        /// <param name="date"></param>
        public void DayButtonClicked(DateTime date)
        {
            SelectedDate = date;

            if (Ref_DatePicker_DateRange != null)
            {
                Ref_DatePicker_DateRange.DateSelected(date);
            }

            if (Config.Events.OnDaySelected != null)
            {
                Config.Events.OnDaySelected.Invoke(date);
            }
        }

        /// <summary>
        /// Called by DayButton
        /// </summary>
        /// <param name="date"></param>
        public void DayButtonMouseOver(DateTime date)
        {
            if (Config.Events.OnDayMouseOver != null)
            {
                Config.Events.OnDayMouseOver.Invoke(date);
            }
        }

        /// <summary>
        /// Called by the screen overlay when it is clicked
        /// </summary>
        public void ModalOverlayClicked()
        {
            if (Ref_DatePicker_DateRange != null)
            {
                Ref_DatePicker_DateRange.ModalOverlayClicked();
            }
            else
            {
                if (Config.Modal.CloseWhenModalOverlayClicked) Hide();
            }
        }

        public void InputFieldClicked()
        {
            if (Config.InputField.ToggleDisplayWhenInputFieldClicked) ToggleDisplay();
        }

        public void ToggleDisplay()
        {            
            if (Ref_DatePickerTransform.gameObject.activeInHierarchy)
            {
                Hide();
            }
            else
            {                
                Show();
            }
        }

        public void Show()
        {
            var canvas = FindParentOfType<Canvas>(this.gameObject);

            if (Ref_InputField != null)
            {
                // Position tablelayout relative to InputField
                SetPositionAdjacentToInputFieldContainer();                
            }

            Ref_DatePickerTransform.gameObject.SetActive(true);            

            if (Config.Modal.IsModal && Ref_ScreenOverlay != null)
            {
                if (canvas != null)
                {
                    Ref_ScreenOverlay.transform.SetParent(canvas.transform);
                    Ref_ScreenOverlay.transform.SetAsLastSibling();
                }

                Ref_ScreenOverlay.gameObject.SetActive(true);

                Ref_ScreenOverlayAnimator.PlayAnimation(Animation.Fade, AnimationType.Show);
            }

            if (this.transform != Ref_DatePickerTransform)
            {                
                if (canvas != null)
                {
                    Ref_DatePickerTransform.SetParent(canvas.transform);
                    Ref_DatePickerTransform.SetAsLastSibling();
                }
            }

            if (Config.Animation.ShowAnimation != Animation.None)
            {
                PlayAnimation(Config.Animation.ShowAnimation, AnimationType.Show);
            }
        }

        private void PlayAnimation(Animation animation, AnimationType animationType, Action onComplete = null)
        {
            Ref_Animator.PlayAnimation(animation, animationType, onComplete);            
        }        

        public void Hide()
        {
            if (Config.Animation.HideAnimation != Animation.None)
            {
                PlayAnimation(Config.Animation.HideAnimation, AnimationType.Hide, _Hide);
            }
            else
            {
                _Hide();
            }            
        }

        private void _Hide()
        {
            if (Config.Modal.IsModal)
            {
                if(Ref_ScreenOverlay != null) Ref_ScreenOverlayAnimator.PlayAnimation(Animation.Fade, AnimationType.Hide, HideScreenOverlay_Complete);
            }

            if (this.transform != Ref_DatePickerTransform)
            {
                Ref_DatePickerTransform.SetParent(this.transform);                
            }

            Ref_DatePickerTransform.gameObject.SetActive(false);
        }

        private void HideScreenOverlay_Complete()
        {
            Ref_ScreenOverlay.transform.SetParent(this.transform);            
            Ref_ScreenOverlay.gameObject.SetActive(false);
        }

        private void SetPositionAdjacentToInputFieldContainer()
        {            
            if (Ref_InputFieldContainer == null) return;            

            var rectTransform = Ref_DatePickerTransform;
            var inputFieldRectTransform = Ref_InputFieldContainer.transform as RectTransform;
            var inputFieldWidth = inputFieldRectTransform.rect.width;

            if (Config.Sizing.UsePreferredWidthInsteadOfInputFieldWidth)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Config.Sizing.PreferredWidth);
            }
            else
            {                
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inputFieldWidth);
            }
            

            var pivotX = 0.5f;            
            switch (Config.InputField.DatePickerAlignmentRelativeToInputField)
            {
                case Alignment.Left:
                    {
                        pivotX = 0f;                        
                    }
                    break;
                case Alignment.Right:
                    {
                        pivotX = 1f;                        
                    }
                    break;
            }            

            var canvasTransform = ((RectTransform)GetComponentInParent<Canvas>().transform);
            Vector3[] canvasCorners = new Vector3[4], inputFieldCorners = new Vector3[4];
            canvasTransform.GetWorldCorners(canvasCorners);
            inputFieldRectTransform.GetWorldCorners(inputFieldCorners);            

            var distanceFromCanvasBottom = Mathf.Abs(canvasCorners[3].y - inputFieldCorners[3].y);

            var actualHeight = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale).x;            

            calculatedPosition = actualHeight > distanceFromCanvasBottom ? DatePickerPosition.Above : DatePickerPosition.Below;

            if (calculatedPosition == DatePickerPosition.Above)
            {
                rectTransform.pivot = new Vector2(pivotX, 0);                                                                
                rectTransform.anchoredPosition = inputFieldRectTransform.anchoredPosition;

                // if, in the Above position, we're so high that we go above the top of the canvas, then apply an offset to correct this
                var offsetY = canvasTransform.rect.height - (inputFieldCorners[2].y + rectTransform.rect.height);
                if (offsetY < 0)
                {
                    rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y + offsetY, rectTransform.position.z);
                }
            }
            else
            {                
                rectTransform.pivot = new Vector2(pivotX, 1);
                rectTransform.anchoredPosition = inputFieldRectTransform.anchoredPosition;
                rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y - inputFieldRectTransform.rect.height, rectTransform.position.z);
            }            

            DatePickerTimer.DelayedCall(0.05f, () => { Ref_DayTableContainer.GetRow().NotifyTableRowPropertiesChanged(); }, this);            
        }

        private static T FindParentOfType<T>(GameObject childObject)
        where T : UnityEngine.Object
        {
            Transform t = childObject.transform;
            while (t.parent != null)
            {
                var component = t.parent.GetComponent<T>();

                if (component != null) return component;

                t = t.parent.transform;
            }

            // We didn't find anything
            return null;
        }

        #region SetProperty
        protected void SetProperty<T>(ref T currentValue, T newValue)
        {            
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return;
            currentValue = newValue;

            UpdateDisplay();
        }

        void OnRectTransformDimensionsChange()
        {                        
            //UpdateDisplay();
            DatePickerTimer.DelayedCall(0.025f, UpdateDisplay, this);
        }
        #endregion              
    }        
}
