using Windows.ApplicationModel.Preview.Holographic;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;

namespace Huyn
{
    public sealed class WindowDisplayInfo
    {
        private static bool IsMixedReality()
        {
            try
            {
                return ApiInformation.IsTypePresent(
                        "Windows.ApplicationModel.Preview.Holographic.HolographicApplicationPreview")
                    && ApiInformation.IsMethodPresent(
                        "Windows.ApplicationModel.Preview.Holographic.HolographicApplicationPreview",
                        nameof(HolographicApplicationPreview.IsCurrentViewPresentedOnHolographicDisplay))
                    && HolographicApplicationPreview.IsCurrentViewPresentedOnHolographicDisplay();
            }
            catch
            {
            }
            return false;
        }

        public static WindowDisplayMode GetForCurrentView()
        {
            ApplicationView applicationView = ApplicationView.GetForCurrentView();

            switch (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                case "Windows.Desktop":
                    {
                        if (applicationView.ViewMode == ApplicationViewMode.CompactOverlay)
                        {
                            return WindowDisplayMode.CompactOverlay;
                        }
                        if (IsMixedReality())
                        {
                            return WindowDisplayMode.Windowed;
                        }
                        else
                        {
                            if (applicationView.IsFullScreenMode)
                            {
                                return WindowDisplayMode.FullScreen;
                            }
                            else
                            {
                                switch (UIViewSettings.GetForCurrentView().UserInteractionMode)
                                {
                                    case UserInteractionMode.Mouse:
#pragma warning disable CS0618 // Type or member is obsolete
                                        return applicationView.IsFullScreen ? WindowDisplayMode.Maximized : WindowDisplayMode.Windowed;
#pragma warning restore CS0618 // Type or member is obsolete
                                    case UserInteractionMode.Touch:
                                        {
                                            if (applicationView.AdjacentToLeftDisplayEdge)
                                            {
                                                if (applicationView.AdjacentToRightDisplayEdge)
                                                {
                                                    return WindowDisplayMode.FullScreenTabletMode;
                                                }
                                                else
                                                {
                                                    return WindowDisplayMode.SnappedLeft;
                                                }
                                            }
                                            else
                                            {
                                                return WindowDisplayMode.SnappedRight;
                                            }
                                        }
                                    default:
                                        return WindowDisplayMode.Unknown;
                                }
                            }
                        }
                    }
                case "Windows.Mobile":
                    {
                        if (UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Mouse)
                        {
                            // Continuum
                            return applicationView.IsFullScreenMode ? WindowDisplayMode.Maximized : WindowDisplayMode.Windowed;
                        }
                        else
                        {
                            return WindowDisplayMode.FullScreen;
                        }
                    }
                case "Windows.Holographic":
                    {
                        return WindowDisplayMode.Windowed;
                    }
                case "Windows.Xbox":
                case "Windows.IoT":
                    {
                        return WindowDisplayMode.FullScreen;
                    }
                case "Windows.Team":
                    {
                        if (applicationView.AdjacentToLeftDisplayEdge)
                        {
                            if (applicationView.AdjacentToRightDisplayEdge)
                            {
                                return WindowDisplayMode.FullScreenTabletMode;
                            }
                            else
                            {
                                return WindowDisplayMode.SnappedLeft;
                            }
                        }
                        else
                        {
                            return WindowDisplayMode.SnappedRight;
                        }
                    }
                default:
                    return WindowDisplayMode.Unknown;
            }
        }


    }
}
