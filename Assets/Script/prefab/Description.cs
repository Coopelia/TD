public class Description
{
    public static string GetTurretDescription(TurretType _type)
    {
        string description;
        switch (_type)
        {
            case TurretType.LingYuan_lv1:
                description = "一级泠鸢（魔法师）";
                break;
            case TurretType.LingYuan_lv2_1:
                description = "二级泠鸢（魔法师）";
                break;
            case TurretType.LingYuan_lv2_2:
                description = "二级泠鸢（射手）";
                break;
            case TurretType.Hanser_lv1:
                description = "一级hanser（射手）";
                break;
            case TurretType.Hanser_lv2:
                description = "二级hanser（射手）";
                break;
            case TurretType.Amiya_lv1:
                description = "一级阿米驴（魔法师）";
                break;
            case TurretType.Amiya_lv2:
                description = "二级阿米驴（魔法师）";
                break;
            case TurretType.Enterprise_lv1:
                description = "一级E·P（魔法师）";
                break;
            case TurretType.Enterprise_lv2_1:
                description = "二级E·P（射手）";
                break;
            case TurretType.Enterprise_lv2_2:
                description = "二级E·P（魔法师）";
                break;
            case TurretType.Enterprise_lv3_1:
                description = "三级E·P（射手）";
                break;
            case TurretType.Enterprise_lv3_2:
                description = "三级E·P（魔法师）";
                break;
            default:
                description = "暂无描述";
                break;
        }
        return description;
    }

    public static string GetEquipmentDescription(int id)
    {
        string description;
        switch (id)
        {
            case 0:
                description = "貌似是一把普普通通的剑，但是放到莫个人手里说不定能发挥巨大的作用。";
                break;
            case 1:
                description = "传说中亚瑟王使用的剑，只有在释放必杀技时才会显现出外形。";
                break;
            case 2:
                description = "流浪大地的孤单少女在战斗中死去，陪伴她的法杖孤立在遗体身旁，坚信着这趟旅途并未结束……";
                break;
            case 3:
                description = "给你点个赞！";
                break;
            case 4:
                description = "某位王戴过的戒指，即使已经残破，但依然能微微感受到蕴藏在里面的力量。";
                break;
            case 5:
                description = "拥有坚定信念的勇者，女神会降临在他身边，送出真挚的祝福。";
                break;
            case 6:
                description = "世界上产量最高的枪械。";
                break;
            case 7:
                description = "雌雄双剑，复仇之刃！~复仇之刃。。。。。";
                break;
            case 8:
                description = "死亡圣器之一，传闻可以令主人战无不胜。";
                break;
            default:
                description = "暂无描述";
                break;
        }
        return description;
    }
}
