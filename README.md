# Paint

![alt text](https://i.imgur.com/xZfDTpq.png)

## Thông tin nhóm

| MSSV     | Họ và tên       |
| -------- | --------------- |
| 18120256 | Vũ Lê Tuấn      |
| 18120305 | Lê Quốc Đạt     |
| 18120467 | Huỳnh Quang Nam |

## Hướng dẫn chạy project

- Mở solution bằng Visual Studio 2022. (.NET 6.0)
- Solution gồm:
  -- Paint: project chính của chương trình.
  -- PaintContract: project chứa các interface hợp đồng.
  -- PaintLibrary: lần lượt là các thư viện của chương trình gồm các luật đổi tên.
- Nhấn tổ hợp phím `Ctrl + shift + B` để build project.
- Tiến hành copy PaintLibrary.dll vào folder chứa file .exe của chương trình chính.

## Bảng phân công

| STT | Họ và tên       | Chức năng                                                                                                                                                                                                                                                                                                                                                                                                                                  | Đánh giá (h) |
| --- | --------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------ |
| 1   | Lê Quốc Đạt     | - Dynamically load all graphic objects that can be drawn from external DLL files.<br> - The user can see the preview of the object they want to draw. <br> - Select a single element for editing again. Transforming horizontally and vertically. Drag & Drop. <br>- Resize object by 4 corner. <br> - Zoom in, zoom out. <br> - Create list layer. <br> - Adding text to the list of drawable objects. <br> - Adding image to the canvas. | 28           |
| 2   | Huỳnh Quang Nam |
| 3   | Vũ Lê Tuấn      | - The list of drawn objects can be saved and loaded again for continuing later <br> - Save and load all drawn objects as an image in bmp/png/jpg format (rasterization) <br> - Apply builder design pattern to select shape and config <br> - Allow user to draw with pen <br> - Base layer design                                                                                                                                         | 28           |

## Kiến trúc và kỹ thuật

- Design patterns: Singleton, Factory Method, Prototype, Builder.
- Plugin architecture.
- Delegate & event.
- C#, WPF.

## Danh sách chức năng đã hoàn thành.

### Core requirements (6/6)

1.  Dynamically load all graphic objects that can be drawn from external DLL files
2.  The user can choose which object to draw
3.  The user can see the preview of the object they want to draw
4.  The user can finish the drawing preview and their change becomes permanent with previously drawn objects
5.  The list of drawn objects can be saved and loaded again for continuing later
    You must save in your own defined binary format
6.  Save and load all drawn objects as an image in bmp/png/jpg format (rasterization). Just one format is fine. No need to save in all three formats.

### Improvements (8.5/11)

1.  Allow the user to change the color, pen width, stroke type (dash, dot, dash dot dot...\_
2.  Adding text to the list of drawable objects
3.  Adding image to the canvas
5.  Adding Layers support
6.  Select a single element for editing again
    Transforming horizontally and vertically
    Drag & Drop
7.  Zooming
8.  Cut / Copy / Paste
10. Fill color by boundaries
11. Anything that you think is suitable

## Danh sách chức năng chưa hoàn thành

### Improvements

4. Reduce flickering when drawing preview by using buffer to redraw all the canvas
    Upgrade: Only redraw the needed region, no fullscreen redraw
6. Rotate the image
9. Undo, Redo

## Bonus

- Resize đối tượng hình ellipse, rectangle, hình ảnh theo 4 góc.
- Allow user to draw with pen

## Điểm tự đánh giá

10/10

## Github and video demo

- Github: https://github.com/HCMUS-K18-WINDOWS/Paint
- Video demo:
