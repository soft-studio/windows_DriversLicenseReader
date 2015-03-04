# windows_DriversLicenseReader
Windows用の、運転免許証読み込みプロジェクトです。
VisualStudio2010上で、C#にて開発しました。

This software is released under the MIT License,

動作はPaSoRi(RC-S380)にて確認しましたが、スマートカードAPIを利用しておりますので、他のICカードR/Wでも読めると思います。

顔写真用のJPEG2000デコードには、
「csj2k」https://github.com/cureos/csj2k　
を利用。

スマートカードアクセスには
http://wikiwiki.jp/webapp/?NFC%2FPC_SC%A5%B5%A5%F3%A5%D7%A5%EB
http://eternalwindows.jp/security/scard/scard00.html
を参考にさせていただきました。

エラー処理などは不十分ですので、開発用として慎重にお使いください。

IC運転免許証の仕様は
http://www.npa.go.jp/pdc/notification/koutuu/menkyo/menkyo20150129.pdf
を参考にしております。
