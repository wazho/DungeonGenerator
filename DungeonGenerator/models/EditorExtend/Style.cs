using UnityEngine;
using UnityEditor;
using System.Collections;

namespace EditorExtend {
    public class Style {
        // Font size.
        private const int _headerFontSize  = 24;
        private const int _contentFontSize = 12;
        // Button.
        private const int _tabButtonHeight = 25;
        private const int _buttonHeight    = 20;
        // Canvas.
        private const int _paddingAfterCanvas = 10;
        private static Rect _alphabetPreviewArea      = new Rect(0, 200, Screen.width, 150);
        private static Rect _alphabetPreviewCanvas    = new Rect(0,   0, Screen.width, 150);
        private static Rect _afterAlphabetPreviewArea = new Rect(0, 350, Screen.width, Screen.height);

        // Font size.
        public static int HeaderFontSize {
            get { return _headerFontSize; }
        }
        public static int ContentFontSize {
            get { return _contentFontSize; }
        }
        // Button.
        public static GUILayoutOption TabButtonHeight {
            get { return GUILayout.Height(_tabButtonHeight); }
        }
        public static GUILayoutOption ButtonHeight {
            get { return GUILayout.Height(_buttonHeight); }
        }
        // Canvas.
        public static int PaddingAfterCanvas {
            get { return _paddingAfterCanvas; }
        }
        public static Rect AlphabetPreviewArea {
            get {
                _alphabetPreviewArea.width = Screen.width;
                return _alphabetPreviewArea;
            }
        }
        public static Rect AlphabetPreviewCanvas {
            get {
                _alphabetPreviewCanvas.width = Screen.width;
                return _alphabetPreviewCanvas;
            }
        }
        public static Rect AfterAlphabetPreviewArea {
            get {
                _afterAlphabetPreviewArea.width  = Screen.width;
                _afterAlphabetPreviewArea.height = Screen.height;
                return _afterAlphabetPreviewArea;
            }
        }
    }
}