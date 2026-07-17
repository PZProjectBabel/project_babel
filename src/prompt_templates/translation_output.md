========CRITICAL OUTPUT RULES========
1. 输出必须是**纯文本**。
2. 每行一个翻译结果，格式为
```
<index>	<translation>	<confidence>	[optional_comment]
```
3. <index>必须与翻译条目的序号一致。
4. **严禁**包含 `en`、`multi_lang_context`、`rag_context` 或任何输入中的原始字段。
5. 仅当 `confidence` 为 -1.0 时，才允许额外添加 `comment` 字段。
6. 严格遵守翻译【翻译格式硬规则】
7. 如果译文为空，则输出```<index>      <confidence>```，如示例输出所示
8. 严禁在翻译条目中间换行
9. 严禁用空格代替tab分隔

========EXPECTED OUTPUT========
```
<index>	<translation>	<confidence>	[optional_comment]
```

========EXPECTED OUTPUT EXAMPLE========
```
T1	Target translation 1	0.99
T2	Target translation 2	0.95
T3	Target translation 3	-1.00	Target-language conflict note
T4	Target translation 4	0.30
T5      1.00
...
```
