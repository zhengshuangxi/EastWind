using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class XmlParser
{
    public static Result Parse(string content)
    {
        Result result = new Result();

#if Release
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(content);
        XmlNodeList chapter = xml.GetElementsByTagName("read_chapter");
        if (chapter.Count > 0)
        {
            XmlNode chaperNode = chapter.Item(0);
            result.content = chaperNode.Attributes["content"].Value;

            if (chaperNode.Attributes["is_rejected"].Value == "true")
            {
                result.error = Error.NOT_BY_RULE;
                result.score.total = 0f;
            }

            if (chaperNode.Attributes["except_info"].Value != "0")
            {
                if (chaperNode.Attributes["except_info"].Value == "28673")
                    result.error = Error.VOLUME_LOW;
                if (chaperNode.Attributes["except_info"].Value == "28676")
                    result.error = Error.NOT_BY_RULE;
                if (chaperNode.Attributes["except_info"].Value == "28680")
                    result.error = Error.SNR_LOW;
                if (chaperNode.Attributes["except_info"].Value == "28690")
                    result.error = Error.CLIPPING;
                else
                    result.error = Error.OTHER;
                result.score.total = 0f;
            }

            result.score.total = float.Parse(chaperNode.Attributes["total_score"].Value);
            if (result.score.total <= 2f)
                result.error = Error.NOT_BY_RULE;
        }
        else
        {
            result.error = Error.OTHER;
        }
#else
        result.score.total = 5f;
#endif

        return result;
    }
}