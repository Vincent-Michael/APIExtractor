--
-- Table structure for table `quest_api`
--

DROP TABLE IF EXISTS `quest_api`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_api` (
  `id` int(10) unsigned NOT NULL,
  `title` text NOT NULL,
  `reqLevel` smallint(5) NOT NULL,
  `suggestedPartyMembers` smallint(5) NOT NULL,
  `category` text NOT NULL,
  `level` smallint(5) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
