using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PagedList
{

        public class PagedListRenderOptions : PagedList.Mvc.PagedListRenderOptions
        {
            ///<summary>
            /// The default settings render all navigation links and no descriptive text.
            ///</summary>
            public PagedListRenderOptions()
            {
                //dont change any of these values, instead, make a new static method below and override
                DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded;
                DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded;
                DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded;
                DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded;
                DisplayLinkToIndividualPages = true;
                DisplayPageCountAndCurrentLocation = false;
                MaximumPageNumbersToDisplay = 10;
                DisplayEllipsesWhenNotShowingAllPageNumbers = true;
                EllipsesFormat = "&#8230;";
                LinkToFirstPageFormat = "««";
                LinkToPreviousPageFormat = "«";
                LinkToIndividualPageFormat = "{0}";
                LinkToNextPageFormat = "»";
                LinkToLastPageFormat = "»»";
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
                FunctionToDisplayEachPageNumber = null;
                ClassToApplyToFirstListItemInPager = null;
                ClassToApplyToLastListItemInPager = null;
                ContainerDivClasses = new[] { "pagination-container" };
                UlElementClasses = new[] { "pagination" };
                LiElementClasses = Enumerable.Empty<string>();
            }

            public static PagedListRenderOptions tomsLayout
            {
                get
                {
                return new PagedListRenderOptions
                 {
                          DisplayLinkToFirstPage = PagedListDisplayMode.Always, 
                          DisplayLinkToLastPage = PagedListDisplayMode.Always,
                          DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                          DisplayLinkToNextPage = PagedListDisplayMode.Always,
                          MaximumPageNumbersToDisplay = 4
                };

            }
        }
        }
    }

