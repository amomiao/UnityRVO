1.导入后报错 BurstCompileAttribute 等有Burst内容的。
(1)删掉TheRVO/Scripts/RVOJob文件夹,然后删除FixNum和FixVector3的报错特性
(2)或者导入JobSystem相关的Pack 

2.运行演示
(1)运行场景
(2)Game窗口右上角Gizmos打开
(3)鼠标点击Game中任意位置,代理走向那个点
(4)按住左Shift时，可以逐个分配代理实体的目标点 

-Example
	ExampleAgentMono:实现代理一个的案例,挂到物体上就行
	ExampleAgentMono:实现代理系统一个的案例,挂到物体上就行
-Obsolete 已废弃
	ExampleRVOForward:已废弃
	AgentControllerRVO:已废弃
AgentLineAreaRVO 线性规划计算器
	1.记录构成点斜参数
	public readonly FixVector3 originalVector;
	public readonly FixVector3 originalPoint;
	2.并点斜式构建ax+by+c=0.
 	public readonly FixNum a;
    public readonly FixNum b;
    public readonly FixNum c;
    3.验证点在线性规划中的合法性
    public bool IsLegal(FixVector3 point)
AgentRVO 代理计算器
	1. 启动
	public void UpdateSpeed(FixNum velocity)
	2. 停止
	public void Stop()
	3. 是否需要避障,需要时会赋值一个线性规划参数
	public bool Avoid(AgentRVO b, FixNum deltaTime, ref AgentLineAreaRVO line)
	4. 修正移动朝向实现避障,需要一个线性规划参数
	public void CorrectDirectionVector(FixNum deltaTime, AgentLineAreaRVO line)