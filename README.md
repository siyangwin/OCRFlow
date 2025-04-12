![本地图片结果](https://github.com/user-attachments/assets/085040c3-4168-4c5a-9594-0e0296538a78)# OCRFlow .NET Core API

OCRFlow 是一个基于 [PaddleOCR]的简易 OCR 识别服务，使用 .NET Core 开发，内置 Swagger，支持普通模式和表格模式的图片识别。它支持接收本地图片路径和网络图片 URL，通过 API 提供 OCR 识别功能。

## 功能

- **普通模式识别**：用于处理普通文本图片的 OCR 任务。
- **表格模式识别**：专门针对表格内容进行 OCR 识别。
- 支持本地图片路径和网络图片 URL 作为输入。

## 启动应用

启动项目后，应用会自动启动 Swagger UI，访问以下地址即可进行接口测试：

http://localhost:8081/index.html

## API 接口

### **OCR 识别**

#### 请求 URL

```bash
POST /api/ocr/Check


#### 请求参数

| 参数   | 类型   | 描述                                   |
|--------|--------|----------------------------------------|
| type   | int    | 识别类型：`1` - 普通模式，`2` - 表格模式 |
| path   | string | 本地图片路径或网络图片 URL              |

#### 示例请求

http://localhost:8081/api/ocr/Check?type=1&path=D:\OCR\5.png


#### 返回参数

```json
{
  "api_version": "v1",
  "success": true,
  "code": "200",
  "message": "耗时：184.8024ms\n",
  "data": "[{\"BoxPoints\":[{\"X\":43,\"Y\":318},{\"X\":566,\"Y\":333},{\"X\":563,\"Y\":441},{\"X\":40,\"Y\":427}],\"Score\":0.996798038482666,\"Text\":\"鲁ND6666\",\"cls_label\":-1,\"cls_score\":0.0},{\"BoxPoints\":[{\"X\":32,\"Y\":495},{\"X\":584,\"Y\":500},{\"X\":583,\"Y\":631},{\"X\":31,\"Y\":626}],\"Score\":0.9457632899284363,\"Text\":\"鲁NND6666\",\"cls_label\":-1,\"cls_score\":0.0}]"
}
```

### 示例
1. 网络图片: https://i1.hdslb.com/bfs/archive/50c44178b111966f827f60a686641f4c49ea5571.jpg
   ![网络图片示例](https://github.com/user-attachments/assets/7d062efd-c2e6-48e1-888a-7f951a8a8d51)
   识别结果：
   ![网络图片结果](https://github.com/user-attachments/assets/5818c712-2d2c-4fa0-88f8-9ef938936988)

2.本地图片：
  ![本地图片示例](https://github.com/user-attachments/assets/5fc2ecd0-67fc-4ab9-afac-4b2043e54abe)
  识别结果：
  ![本地图片结果](https://github.com/user-attachments/assets/b42f0df8-2af2-45b0-af14-bbb11834afa1)








