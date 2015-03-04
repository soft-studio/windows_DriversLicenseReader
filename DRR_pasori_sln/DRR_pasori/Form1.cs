/*
The MIT License (MIT)

Copyright (c) <2015> <Soft-Studio K.K. info@soft-studio.jp>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSJ2K;
using System.Text.RegularExpressions;

namespace DRR_pasori
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBoxPass1.Text.Length == 0)
            {
                MessageBox.Show("暗証番号１が未入力です");
                return;
            }
            if (textBoxPass1.Text.Length < 4)
            {
                MessageBox.Show("暗証番号１は４桁必要です");
                return;
            }
            if (!((Regex.Match(textBoxPass1.Text, "^[a-zA-Z0-9¥*]+$")).Success))
            {
                MessageBox.Show("暗証番号１に、使用できない文字が含まれています");
                return;
            }
            if (textBoxPass2.Text.Length == 0)
            {
                MessageBox.Show("暗証番号２が未入力です");
                return;
            }
            if (textBoxPass2.Text.Length < 4)
            {
                MessageBox.Show("暗証番号２は４桁必要です");
                return;
            }
            if (!((Regex.Match(textBoxPass2.Text, "^[a-zA-Z0-9¥*]+$")).Success))
            {
                MessageBox.Show("暗証番号２に、使用できない文字が含まれています");
                return;
            }

            Scard sc = new Scard();
            bool ret = sc.start(textBoxPass1.Text, textBoxPass2.Text);
            if (ret == false)
            {
                MessageBox.Show(sc.errormsg);
                return;
            }
            labelName.Text = sc.name;
            labelYomikana.Text = sc.kana;
            labelTusyo.Text = sc.tusyo;
            labelToitu.Text = sc.toitsu;
            labelBirth.Text = ConvertNego(sc.birth);
            labelAdr.Text = sc.address;
            labelHonseki.Text = sc.honseki;
            labelKubun.Text = sc.kubun;
            labelNumber.Text = sc.menkyonumber;
            labelJyoken1.Text = sc.joken1;
            labelJyoken2.Text = sc.joken2;
            labelJyoken3.Text = sc.joken3;
            labelJyoken4.Text = sc.joken4;
            labelKigen.Text = ConvertNego(sc.yukoday);
            labelKoan.Text = sc.koanname;

            labelNisyogen.Text = ConvertNego(sc.nisyogenday);    // 免許の年月日(二・小・原)(元号(注6)YYMMDD)
            labelHoka.Text = ConvertNego(sc.hokaday);            // 免許の年月日(他)(元号(注6)YYMMDD)(注9
            labelNisyu.Text = ConvertNego(sc.nisyuday);          // 免許の年月日(二種)(元号(注6)YYMMDD)(注9)
            labelOgata.Text = ConvertNego(sc.ogataday);          // 免許の年月日(大型)(元号(注6)YYMMDD)(注9)
            labelFutu.Text = ConvertNego(sc.futuday);            // 免許の年月日(普通)(元号(注6)YYMMDD)(注9)
            labelDaitoku.Text = ConvertNego(sc.daitokuday);      // 免許の年月日(大特)(元号(注6)YYMMDD)(注9)
            labelDaijini.Text = ConvertNego(sc.daijiniday);      // 免許の年月日(大自二)(元号(注6)YYMMDD)(注9)
            labelFujini.Text = ConvertNego(sc.futujiniday);      // 免許の年月日(普自二)(元号(注6)YYMMDD)(注9)
            labelSyotoku.Text = ConvertNego(sc.kotokuday);       // 免許の年月日(小特)(元号(注6)YYMMDD)(注9)
            labelGentuki.Text = ConvertNego(sc.gentukiday);      // 免許の年月日(原付)(元号(注6)YYMMDD)(注9)
            labelKenbiki.Text = ConvertNego(sc.keninday);        // 免許の年月日(け引)(元号(注6)YYMMDD)(注9)
            labelDaini.Text = ConvertNego(sc.daijiday);          // 免許の年月日(大二)(元号(注6)YYMMDD)(注9)   
            labelDaitokuni.Text = ConvertNego(sc.daitokuji);     // 免許の年月日(大特二)(元号(注6)YYMMDD)(注9)           
            labelkehikini.Text = ConvertNego(sc.keninniday);     // 免許の年月日(け引二)(元号(注6)YYMMDD)(注9)
            labelChugata.Text = ConvertNego(sc.chuday);          // 免許の年月日(中型)(元号(注6)YYMMDD)(注9,注12)
            labelChuni.Text = ConvertNego(sc.chuniday);          // 免許の年月日(中二)(元号(注6)YYMMDD)(注9,注12)
            labelFuni.Text = ConvertNego(sc.fujiday);          // 免許の年月日(普二)(元号(注6)YYMMDD)(注9)    

            Bitmap img = (Bitmap)(J2kImage.FromBytes(sc.picture));
            pictureBox1.Image = img;
        }

        private String ConvertNego(String topone)
        {
            String ret = "昭和";
            String one = topone.Substring(0, 1);
            String year = topone.Substring(1,2);
            String month = topone.Substring(3, 2);
            String day = topone.Substring(5, 2);

            if (one.CompareTo("1") == 0)
            {
                ret = "明治";
            }
            if (one.CompareTo("2") == 0)
            {
                ret = "大正";
            }
            if (one.CompareTo("3") == 0)
            {
                ret = "昭和";
            }
            if (one.CompareTo("4") == 0)
            {
                ret = "平成";
            }
            ret = ret + year + "年" + month + "月" + day + "日";

            return ret;
        }




    }
}
