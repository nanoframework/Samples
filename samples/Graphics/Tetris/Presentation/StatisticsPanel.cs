//-----------------------------------------------------------------------------
// 
//  Tetris game for .NET Micro Framework
//
//  http://bansky.net/blog
// 
// This code was written by Pavel Bansky. It is released under the terms of 
// the Creative Commons "Attribution NonCommercial ShareAlike 2.5" license.
// http://creativecommons.org/licenses/by-nc-sa/2.5/
//-----------------------------------------------------------------------------

using Tetris;
using Tetris.GameLogic;
using nanoFramework.Presentation.Controls;
using nanoFramework.Presentation.Media;
using nanoFramework.Presentation;

namespace Tetris.Presentation
{
    /// <summary>
    /// Panel to display game statistics
    /// </summary>
    public class StatisticsPanel : Panel
    {
        GameStatistics _stats;
        Text scoreCaption;
        Text scoreLabel;
        Text levelCaption;
        Text levelLabel;
        Text linesCaption;
        Text linesLabel;
        StackPanel mainStack;

        /// <summary>
        /// Creates new statistics panel for given statistics
        /// </summary>
        /// <param name="gameStats">GameStatistics</param>
        public StatisticsPanel(GameStatistics gameStats)
        {
            this._stats = gameStats;
            InitializeComponents();
        }

        /// <summary>
        /// Creates all WPF controls of the element
        /// </summary>
        private void InitializeComponents()
        {
            levelCaption = new Text(nfResource.GetString(nfResource.StringResources.Level));
            levelCaption.Font = nfResource.GetFont(nfResource.FontResources.NinaB);
            levelCaption.HorizontalAlignment = HorizontalAlignment.Right;
            levelCaption.ForeColor = Color.White;

            levelLabel = new Text("0");
            levelLabel.Font = nfResource.GetFont(nfResource.FontResources.NinaB);
            levelLabel.HorizontalAlignment = HorizontalAlignment.Right;
            levelLabel.ForeColor = Color.White;

            linesCaption = new Text(nfResource.GetString(nfResource.StringResources.LinesCompleted));
            linesCaption.Font = nfResource.GetFont(nfResource.FontResources.NinaB);
            linesCaption.HorizontalAlignment = HorizontalAlignment.Right;
            linesCaption.ForeColor = Color.White;

            linesLabel = new Text("0");
            linesLabel.Font = nfResource.GetFont(nfResource.FontResources.NinaB);
            linesLabel.HorizontalAlignment = HorizontalAlignment.Right;
            linesLabel.ForeColor = Color.White;

            scoreCaption = new Text(nfResource.GetString(nfResource.StringResources.Score));
            scoreCaption.Font = nfResource.GetFont(nfResource.FontResources.NinaB);
            scoreCaption.HorizontalAlignment = HorizontalAlignment.Right;
            scoreCaption.ForeColor = Color.White;

            scoreLabel = new Text("0");
            scoreLabel.Font = nfResource.GetFont(nfResource.FontResources.NinaB);
            scoreLabel.HorizontalAlignment = HorizontalAlignment.Right;
            scoreLabel.ForeColor = Color.White;

            mainStack = new StackPanel(Orientation.Vertical);
            mainStack.Children.Add(levelCaption);
            mainStack.Children.Add(levelLabel);
            mainStack.Children.Add(linesCaption);
            mainStack.Children.Add(linesLabel);
            mainStack.Children.Add(scoreCaption);
            mainStack.Children.Add(scoreLabel);

            this.Children.Add(mainStack);
        }
        
        public override void OnRender(DrawingContext dc)
        {
            // Update data on render
            scoreLabel.TextContent = _stats.Score.ToString();
            levelLabel.TextContent = _stats.Level.ToString();
            linesLabel.TextContent = _stats.LinesCompleted.ToString();
            base.OnRender(dc);
        }
    }
}
