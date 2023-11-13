# 🌶️🌶️ - Graphics Primitives

This demonstrates the low level graphic functions that are available.
These can be used directly without using the WPF graphics model.

The sample is [located here](./).

Very useful for memory constrained devices with smaller screens.

- ESP32 without PSRAM

The primitive features demonstrated are:-

- Draw Lines and colours.
- Load, rotate and scale bitmap images.
- Display colour gradients.
- Draw shapes.
- Text display and formatting.
- Working with pixcels.
- Clipping.
- Matrix rain demo.

## Adding icon graphics

By following these instructions you can add icons into your graphics application.

1. Navigate to [Fontello](https://fontello.com/)
2. Select icons you would like to use in your application (_Please note, if you want to use icons other than the ones provided, you can also upload SVG-font to the site_)
3. Once the icons are selected, navigate to 'Customize Codes' -tab, where you can see all the selected icons
4. Assign a single letter (like 'A', 'b') to each of the icons (_note, tinyfont does not support letter ranges_). Letter assignment can be done on top of each icon
5. Once you have finished assigning the letters to the icons, give font a name and download it by pressing the "Download webfont" -button
6. Open the download zip, extract the ttf-file from it, and install the font to you your PC by double clicking it
7. (**Optional**) _Open Word and use the font, you just installed, to create icon-letter -map_
8. [Download Tiny Font Tool GUI](http://informatix.miloush.net/microframework/Utilities/TinyFontTool.aspx)
9. Use the tool to convert your icon-font to tinyfont
10. Add newly created tinyfont to your projects resource and use it
