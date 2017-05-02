--
-- Table structure for table `spell_api`
--

DROP TABLE IF EXISTS `spell_api`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `spell_api` (
  `id` int(10) unsigned NOT NULL,
  `name` text NOT NULL,
  `icon` text NOT NULL,
  `description` text NOT NULL,
  `powerCost` text NOT NULL,
  `castTime` text NOT NULL,
  `cooldown` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
