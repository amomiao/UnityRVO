1.导入后报错 BurstCompileAttribute 等有Burst内容的。 <br>
(1)删掉TheRVO/Scripts/RVOJob文件夹,然后删除FixNum和FixVector3的报错特性 <br>
(2)或者导入JobSystem相关的Pack <br>

2.打开场景TheRVO/Scenes/RVO <br>
(1)运行场景 <br>
(2)Game窗口右上角Gizmos打开 <br>
(3)鼠标点击Game中任意位置,运行 <br>
(4)按住左Shift时，可以逐个分配代理实体的目标点 <br>

3.代码建议 <br>
(1) /Scripts/Obsolete中是过时代码,可以删除。 <br>
(2) /Scritps/AgentRVO.cs API <br>
	1) 有一些Get Set <br>
	2) public bool Avoid(AgentRVO b, FixNum deltaTime, ref AgentLineAreaRVO line) <br>
		返回是否与另一个代理将相撞, <br>
		当返回true时line才有意义,line是直线，直线一边的点合法一边不合法。 <br>
	3) public void CorrectDirectionVector(FixNum deltaTime, AgentLineAreaRVO line) <br>
		使用一个line修正方向 <br>
(3) ExampleAgentMono 是一个把AgentRVO作为组件使用的 案例实例 <br>
(4) ExampleEntitySystem 案例系统 <br>
	1) private void ToRVO(int index,out List<AgentLineAreaRVO> lines) <br>
		获得了一些线性规划区域 <br>
	2) private void ToCorrectDirection(int index, List<AgentLineAreaRVO> lines) <br>
		并不是一种数学计算,而是一种设定: <br>
		每个线性区域都记录了运算时的一个最佳避障点， <br>
		若有所有线性区域合法的一个最佳避障点，则进行以此避障，否则认为避障失败。 <br>
