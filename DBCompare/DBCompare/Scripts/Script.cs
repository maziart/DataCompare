using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBCompare.Scripts
{
    public class Script
    {
        public List<Token> Tokens { get; private set; }
        private Token LastToken;
        public Script(string script = null)
        {
            Tokens = new List<Token>();
            if (!string.IsNullOrEmpty(script))
                AddScript(script);
        }
        public Script AddScript(string script)
        {
            var tokens = GetSimpleTokens(script);
            var token = new List<string>();
            var state = State.None;
            foreach (var t in tokens)
            {
                if ((state == State.LineComment && !new[] { "\r", "\n" }.Contains(t))
                    || (state == State.MultiComment && t != "*/")
                    || (state == State.String && !t.EndsWith("'")))
                {
                    token.Add(t);
                    continue;
                }
                if ((state == State.LineComment && new[] { "\r", "\n" }.Contains(t))
                    || (state == State.MultiComment && t == "*/")
                    || (state == State.String && t.EndsWith("'")))
                {
                    token.Add(t);
                    AddToken(FindTokenType(state), string.Join("", token));
                    token.Clear();
                    state = State.None;
                    continue;
                }

                if (state == State.None && (t.StartsWith("'") || t.StartsWith("N'")) && !t.EndsWith("'"))
                    state = State.String;
                else if (state == State.None && t == "--")
                    state = State.LineComment;
                else if (state == State.None && t == "/*")
                    state = State.MultiComment;

                if (state != State.None)
                    token.Add(t);
                else
                    AddToken(t);
            }
            if (token.Count > 0)
                AddToken(string.Join("", token));
            return this;
        }

        private static TokenType FindTokenType(State state)
        {
            switch (state)
            {
                case State.String:
                    return TokenType.String;
                case State.LineComment:
                case State.MultiComment:
                    return TokenType.Comment;
                default:
                    return TokenType.Text;
            }
        }
        private static string[] Delimiters = new[] { "--", "/*", "*/" , " ", "\r", "\n", "\t", "(", ")", ",", ".", "=", "*", "/", "+", "-" ,"%"};
        private static List<string> GetSimpleTokens(string script)
        {
            var token = new List<char>();
            var tokens = new List<string>();
            for (int i = 0; i < script.Length; i++)
            {
                var c = script[i];
                var delimiter = GetDelimiter(script, i);
                if (delimiter != null)
                {
                    FlushCurrentToken(token, tokens);
                    i += delimiter.Length - 1;
                    tokens.Add(delimiter);
                }
                else
                {
                    token.Add(c);
                }
            }
            FlushCurrentToken(token, tokens);
            return tokens;
        }
        private static string GetDelimiter(string input, int index)
        {
            foreach (var delimiter in Delimiters)
            {
                var isMatch = true;
                for (int i = 0; i < delimiter.Length; i++)
                {
                    if (index + i >= input.Length || input[index + i] != delimiter[i])
                    {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch)
                    return delimiter;
            }
            return null;
        }

        private static void FlushCurrentToken(List<char> token, List<string> tokens)
        {
            if (token.Count > 0)
            {
                tokens.Add(new string(token.ToArray()));
                token.Clear();
            }
        }
        private enum State
        {
            None,
            String,
            LineComment,
            MultiComment
        }
        public Script AddToken(string token)
        {
            return AddToken(FindTokenType(token), token);
        }
        public Script AddToken(TokenType type, string token)
        {
            var tokenType = type.ToString().ToLower();
            if (LastToken != null && (LastToken.Type == tokenType || Regex.IsMatch(token, @"^\s*$")))
            {
                LastToken.Value += token;
                return this;
            }
            LastToken = new Token { Value = token, Type = tokenType };
            Tokens.Add(LastToken);
            return this;
        }
        public override string ToString()
        {
            return string.Join("", Tokens.Select(n => n.Value));
        }
        #region Keywords
        private static string[] Keywords = new[] 
        { 
            "absolute", "action", "add", "admin", "after", "aggregate", "alter", "as", "asc", "asymmetric",
            "authorization", "backup", "begin", "binary", "bit", "bit_length", "break", "browse", "bulk",
            "by", "call", "cascade", "case", "catalog", "char", "character", "check", "checkpoint", "close",
            "clustered", "collate", "column", "commit", "compute", "connect", "constraint", "contains", 
            "containstable", "continue", "create", "cube", "current", "current_date", "current_time", 
            "cursor", "database", "date", "dbcc", "deallocate", "dec", "decimal", "declare", "default",
            "delete", "deny", "desc", "disk", "distinct", "distributed", "double", "drop", "dump", 
            "dynamic", "else", "end-exec", "end", "errlvl", "escape", "except", "exec", "execute", "exit",
            "external", "fetch", "file", "fillfactor", "first", "float", "for", "foreign", "freetext", 
            "freetexttable", "from", "full", "function", "get", "global", "go", "goto", "grant", "group",
            "having", "holdlock", "hour", "identity", "identity_insert", "identitycol", "if", "immediate",
            "include", "index", "insensitive", "insert", "int", "integer", "intersect", "into", "isolation",
            "key", "kill", "language", "last", "level", "lineno", "load", "local", "merge", "minute", "mod",
            "modify", "national", "nchar", "next", "no", "nocheck", "nonclustered", "none", "numeric", 
            "object", "octet_length", "of", "off", "offsets", "on", "open", "opendatasource", "openquery",
            "openrowset", "openxml", "option", "order", "out", "output", "over", "partial", "partition", 
            "path", "percent", "plan", "precision", "primary", "print", "prior", "proc", "procedure", 
            "public", "raiserror", "range", "read", "readtext", "real", "reconfigure", "recursive", 
            "references", "relative", "replication", "restore", "restrict", "return", "returns", "revert",
            "revoke", "role", "rollback", "rollup", "row", "rowcount", "rowguidcol", "rows", "rule", "save",
            "schema", "scroll", "second", "securityaudit", "select", "session", "set", "sets", "setuser", 
            "shutdown", "smallint", "sql", "state", "statement", "static", "statistics", "symmetric", 
            "system", "table", "tablesample", "textsize", "then", "time", "timestamp", "to", "top", "tran",
            "transaction", "trigger", "truncate", "tsequal", "union", "unique", "update", "updatetext", 
            "use", "using", "values", "varchar", "varying", "view", "waitfor", "when", "where", "while", 
            "with", "without", "writetext",
            "bigint", "binary", "bit", "char", "cursor", "date", "datetime", "datetime2", "datetimeoffset",
            "decimal", "float", "hierarchyid", "image", "int", "money", "nchar", "ntext", "numeric", 
            "nvarchar", "real", "smalldatetime", "smallint", "smallmoney", "sql_variant", "table", "text", 
            "time", "timestamp", "tinyint", "uniqueidentifier", "varbinary", "varchar", "xml"
        };
        private static string[] GrayKeywords = new[] 
        { 
            "(", ")", ",", ".", "=", "*", "/", "+", "-" ,"%","all", "and", "any", "between", "cross", 
            "exists", "in", "inner", "is", "join", "left", "like", "not", "null", "or", "outer", "pivot", 
            "right", "some", "unpivot" 
        };
        private static string[] Functions = new[] 
        { 
            "grouping", "max", "min", "month", "normalize", "sum", "system_user", 
            "@@connections", "@@cpu_busy", "@@cursor_rows", "@@datefirst", "@@dbts", "@@error", 
            "@@fetch_status", "@@identity", "@@idle", "@@io_busy", "@@langid", "@@language", 
            "@@lock_timeout", "@@max_connections", "@@max_precision", "@@nestlevel", "@@options", 
            "@@pack_received", "@@pack_sent", "@@packet_errors", "@@procid", "@@remserver", "@@rowcount", 
            "@@servername", "@@servicename", "@@spid", "@@textsize", "@@timeticks", "@@total_errors",
            "@@total_read", "@@total_write", "@@trancount", "@@version", "abs", "acos", "app_name", "ascii",
            "asin", "atan", "atn2", "avg", "binary_checksum", "cast", "ceiling", 
            "change_tracking_current_version", "change_tracking_is_column_in_mask", "charindex",
            "checksum", "checksum_agg", "coalesce", "col_length", "col_name", "collationproperty", 
            "columnproperty", "columns_updated", "connectionproperty", "convert", "cos", "cot", "count", 
            "count_big", "current_timestamp", "current_user", "cursor_status", "databaseproperty",
            "databasepropertyex", "datalength", "dateadd", "datediff", "datename", "datepart", "day", 
            "db_id", "db_name", "degrees", "difference", "exp", "file_id", "file_name", "filegroup_id",
            "filegroup_name", "filegroupproperty", "fileproperty", "floor", "formatmessage", 
            "fulltextcatalogproperty", "fulltextserviceproperty", "getansinull", "getdate", "getutcdate",
            "grouping_id", "has_dbaccess", "host_id", "host_name", "ident_current", "ident_seed",
            "index_col", "indexkey_property", "indexproperty", "is_member", "is_srvrolemember", "isdate",
            "isnull", "isnumeric", "len", "log", "log10", "lower", "ltrim", "month", "newid", "nullif", 
            "object_id", "object_name", "objectproperty", "original_db_name", "parsename", "patindex", 
            "permissions", "pi", "power", "quotename", "radians", "rand", "replace", "replicate", 
            "reverse", "round", "rtrim", "scope_identity", "serverproperty", "session_user", 
            "sessionproperty", "sign", "sin", "soundex", "space", "sql_variant_property", "sqrt",
            "square", "stats_date", "stdev", "stdevp", "str", "stuff", "substring", "suser_sid",
            "suser_sname", "switchoffset", "sysdatetime", "sysdatetimeoffset", "sysutcdatetime", 
            "tan", "textptr", "textvalid", "todatetimeoffset", "typeproperty", "unicode", "upper",
            "user", "user_id", "user_name", "var", "varp", "year"
        };
        private static string[] Systems = new[]
        {
            "parameters", "sys", "information_schema",
            "fn_virtualservernodes", "fn_trace_getinfo", "fn_rowdumpcracker", "fn_physloccracker", 
            "fn_helpdatatypemap", "fn_listextendedproperty", "fn_enumcurrentprincipals", "fn_my_permissions",
            "fn_virtualfilestats", "fn_servershareddrives", "fn_trace_getfilterinfo", 
            "fn_replgetcolidfrombitmap", "fn_trace_geteventinfo",
            "all_columns", "all_objects", "all_parameters", "all_sql_modules", "all_views", 
            "allocation_units", "assemblies", "assembly_files", "assembly_modules", "assembly_references",
            "assembly_types", "asymmetric_keys", "backup_devices", "certificates", 
            "change_tracking_databases", "change_tracking_tables", "check_constraints", 
            "column_domain_usage", "column_privileges", "column_type_usages", 
            "column_xml_schema_collection_usages", "columns", "computed_columns", "configurations",
            "constraint_column_usage", "constraint_table_usage", "conversation_endpoints", 
            "conversation_groups", "conversation_priorities", "credentials", "crypt_properties", 
            "cryptographic_providers", "data_spaces", "database_audit_specification_details",
            "database_audit_specifications", "database_files", "database_mirroring", 
            "database_mirroring_endpoints", "database_mirroring_witnesses", "database_permissions",
            "database_principal_aliases", "database_principals", "database_recovery_status",
            "database_role_members", "databases", "default_constraints", "destination_data_spaces",
            "dm_audit_actions", "dm_audit_class_type_map", "dm_broker_activated_tasks", 
            "dm_broker_connections", "dm_broker_forwarded_messages", "dm_broker_queue_monitors", 
            "dm_cdc_errors", "dm_cdc_log_scan_sessions", "dm_clr_appdomains", "dm_clr_loaded_assemblies",
            "dm_clr_properties", "dm_clr_tasks", "dm_cryptographic_provider_properties", 
            "dm_database_encryption_keys", "dm_db_file_space_usage", "dm_db_index_usage_stats",
            "dm_db_mirroring_auto_page_repair", "dm_db_mirroring_connections", 
            "dm_db_mirroring_past_actions", "dm_db_missing_index_details", "dm_db_missing_index_group_stats",
            "dm_db_missing_index_groups", "dm_db_partition_stats", "dm_db_persisted_sku_features", 
            "dm_db_script_level", "dm_db_session_space_usage", "dm_db_task_space_usage", 
            "dm_exec_background_job_queue", "dm_exec_background_job_queue_stats", "dm_exec_cached_plans", 
            "dm_exec_connections", "dm_exec_procedure_stats", "dm_exec_query_memory_grants", 
            "dm_exec_query_optimizer_info", "dm_exec_query_resource_semaphores", "dm_exec_query_stats", 
            "dm_exec_query_transformation_stats", "dm_exec_requests", "dm_exec_sessions", 
            "dm_exec_trigger_stats", "dm_filestream_file_io_handles", "dm_filestream_file_io_requests", 
            "dm_fts_active_catalogs", "dm_fts_fdhosts", "dm_fts_index_population", "dm_fts_memory_buffers",
            "dm_fts_memory_pools", "dm_fts_outstanding_batches", "dm_fts_population_ranges", 
            "dm_io_backup_tapes", "dm_io_cluster_shared_drives", "dm_io_pending_io_requests", 
            "dm_os_buffer_descriptors", "dm_os_child_instances", "dm_os_cluster_nodes", 
            "dm_os_dispatcher_pools", "dm_os_dispatchers", "dm_os_hosts", "dm_os_latch_stats", 
            "dm_os_loaded_modules", "dm_os_memory_allocations", "dm_os_memory_brokers", 
            "dm_os_memory_cache_clock_hands", "dm_os_memory_cache_counters", "dm_os_memory_cache_entries",
            "dm_os_memory_cache_hash_tables", "dm_os_memory_clerks", "dm_os_memory_node_access_stats", 
            "dm_os_memory_nodes", "dm_os_memory_objects", "dm_os_memory_pools", "dm_os_nodes", 
            "dm_os_performance_counters", "dm_os_process_memory", "dm_os_ring_buffers", "dm_os_schedulers",
            "dm_os_spinlock_stats", "dm_os_stacks", "dm_os_sublatches", "dm_os_sys_info", 
            "dm_os_sys_memory", "dm_os_tasks", "dm_os_threads", "dm_os_virtual_address_dump", 
            "dm_os_wait_stats", "dm_os_waiting_tasks", "dm_os_worker_local_storage", "dm_os_workers", 
            "dm_qn_subscriptions", "dm_repl_articles", "dm_repl_schemas", "dm_repl_tranhash", 
            "dm_repl_traninfo", "dm_resource_governor_configuration", "dm_resource_governor_resource_pools",
            "dm_resource_governor_workload_groups", "dm_server_audit_status", 
            "dm_tran_active_snapshot_database_transactions", "dm_tran_active_transactions",
            "dm_tran_commit_table", "dm_tran_current_snapshot", "dm_tran_current_transaction", 
            "dm_tran_database_transactions", "dm_tran_locks", "dm_tran_session_transactions", 
            "dm_tran_top_version_generators", "dm_tran_transactions_snapshot", "dm_tran_version_store", 
            "dm_xe_map_values", "dm_xe_object_columns", "dm_xe_objects", "dm_xe_packages", 
            "dm_xe_session_event_actions", "dm_xe_session_events", "dm_xe_session_object_columns", 
            "dm_xe_session_targets", "dm_xe_sessions", "domain_constraints", "domains", 
            "endpoint_webmethods", "endpoints", "event_notification_event_types", "event_notifications", 
            "events", "extended_procedures", "extended_properties", "filegroups", "foreign_key_columns", 
            "foreign_keys", "fulltext_catalogs", "fulltext_document_types", "fulltext_index_catalog_usages",
            "fulltext_index_columns", "fulltext_index_fragments", "fulltext_indexes", "fulltext_languages", 
            "fulltext_stoplists", "fulltext_stopwords", "fulltext_system_stopwords", 
            "function_order_columns", "http_endpoints", "identity_columns", "index_columns", "indexes", 
            "internal_tables", "key_column_usage", "key_constraints", "key_encryptions", "linked_logins", 
            "login_token", "master_files", "master_key_passwords", 
            "message_type_xml_schema_collection_usages", "messages", "module_assembly_usages", 
            "numbered_procedure_parameters", "numbered_procedures", "objects", "openkeys", 
            "parameter_type_usages", "parameter_xml_schema_collection_usages", "parameters", 
            "partition_functions", "partition_parameters", "partition_range_values", "partition_schemes",
            "partitions", "plan_guides", "procedures", "referential_constraints", "remote_logins", 
            "remote_service_bindings", "resource_governor_configuration", "resource_governor_resource_pools",
            "resource_governor_workload_groups", "routes", "routine_columns", "routines", "schemas", 
            "schemata", "securable_classes", "server_assembly_modules", "server_audit_specification_details",
            "server_audit_specifications", "server_audits", "server_event_notifications", 
            "server_event_session_actions", "server_event_session_events", "server_event_session_fields", 
            "server_event_session_targets", "server_event_sessions", "server_events", "server_file_audits",
            "server_permissions", "server_principal_credentials", "server_principals", "server_role_members",
            "server_sql_modules", "server_trigger_events", "server_triggers", "servers", 
            "service_broker_endpoints", "service_contract_message_usages", "service_contract_usages", 
            "service_contracts", "service_message_types", "service_queue_usages", "service_queues", 
            "services", "soap_endpoints", "spatial_index_tessellations", "spatial_indexes", 
            "spatial_reference_systems", "sql_dependencies", "sql_expression_dependencies", "sql_logins", 
            "sql_modules", "stats", "stats_columns", "symmetric_keys", "synonyms", "sysaltfiles", 
            "syscacheobjects", "syscharsets", "syscolumns", "syscomments", "sysconfigures", "sysconstraints",
            "syscurconfigs", "syscursorcolumns", "syscursorrefs", "syscursors", "syscursortables", 
            "sysdatabases", "sysdepends", "sysdevices", "sysfilegroups", "sysfiles", "sysforeignkeys", 
            "sysfulltextcatalogs", "sysindexes", "sysindexkeys", "syslanguages", "syslockinfo", 
            "syslogins", "sysmembers", "sysmessages", "sysobjects", "sysoledbusers", "sysopentapes", 
            "sysperfinfo", "syspermissions", "sysprocesses", "sysprotects", "sysreferences", 
            "sysremotelogins", "sysservers", "system_columns", 
            "system_components_surface_area_configuration", "system_internals_allocation_units", 
            "system_internals_partition_columns", "system_internals_partitions", "system_objects", 
            "system_parameters", "system_sql_modules", "system_views", "systypes", "sysusers", 
            "table_constraints", "table_privileges", "table_types", "tables", "tcp_endpoints", 
            "trace_categories", "trace_columns", "trace_event_bindings", "trace_events", 
            "trace_subclass_values", "traces", "transmission_queue", "trigger_event_types", 
            "trigger_events", "triggers", "type_assembly_usages", "types", "user_token", "via_endpoints",
            "view_column_usage", "view_table_usage", "views", "xml_indexes", "xml_schema_attributes", 
            "xml_schema_collections", "xml_schema_component_placements", "xml_schema_components", 
            "xml_schema_elements", "xml_schema_facets", "xml_schema_model_groups", "xml_schema_namespaces",
            "xml_schema_types", "xml_schema_wildcard_namespaces", "xml_schema_wildcards"
        };
        private static string[] Procedures = new[]
        {
            "sp_executesql", "sp_msforeachtable",
            "sp_msforeach_worker", "sp_msobjectprivs", "sp_validatemergepullsubscription", 
            "sp_mshelpmergeschemaarticles", "sp_addpullsubscription_agent", "sp_helpfilegroup",
            "sp_helpmergepullsubscription", "sp_msscript_sync_ins_trig",
            "sp_mschange_logreader_agent_properties", "sp_describe_cursor_columns",
            "sp_mssetbit", "sp_msdrop_6x_replication_agent", "sp_msvalidate_wellpartitioned_articles",
            "sp_replmonitorhelppublisher", "sp_msdetectinvalidpeerconfiguration", "sp_lock", 
            "sp_syspolicy_update_event_notification", "sp_trace_create", "sp_msadd_merge_subscription", 
            "sp_msmerge_getgencur_public", "sp_helppublicationsync", "sp_msmakemetadataselectproc", 
            "sp_table_privileges_rowset2", "sp_msenum_merge_s", "sp_replcleanupccsprocs", 
            "sp_msunregistersubscription", "sp_msadd_distribution_agent", "sp_restoremergeidentityrange",
            "sp_msrepl_fixpalrole", "sp_mscreatedummygeneration", "sp_msreset_subscription", 
            "sp_xml_schema_rowset", "sp_mschange_subscription_dts_info", "sp_addmergearticle",
            "sp_dbmmonitorupdate", "sp_provider_types_100_rowset", "sp_adddistributiondb",
            "sp_table_privileges_ex", "sp_unbindrule", "sp_article_validation", 
            "sp_startpublication_snapshot", "sp_msenumallpublications", "sp_msaddmergetriggers", 
            "sp_oledbinfo", "sp_indexes_90_rowset2", "sp_dropapprole", 
            "sp_help_log_shipping_secondary_database", "sp_msstartsnapshot_agent", "sp_msaddguidcolumn",
            "sp_cursorprepexec", "sp_requestpeerresponse", "sp_mspublication_access", 
            "sp_msget_new_xact_seqno", "sp_mscheck_logicalrecord_metadatamatch", "sp_createtranpalrole",
            "sp_procedures_rowset2", "sp_ihscriptschfile", "sp_mscheck_agent_instance", 
            "sp_msrepl_snapshot_helparticlecolumns", "sp_mschangedynamicsnapshotjobatdistributor", 
            "sp_replhelp", "sp_mshelptracertokens", "sp_mscdc_ddl_event", "sp_msbrowsesnapshotfolder", 
            "sp_oledb_ro_usrname", "sp_msrepl_check_publisher", "sp_helpdistributor", "sp_fulltext_table",
            "sp_msrepl_schema", "sp_msrepl_backup_start", "sp_msgetlastrecgen", "sp_msdrop_snapshot_dirs",
            "xp_fixeddrives", "sp_msissnapshotitemapplied", "xp_sqlagent_monitor", 
            "sp_msgetmakegenerationapplock", "sp_msupdateinitiallightweightsubscription", 
            "sp_change_tracking_waitforchanges", "sp_msinvalidate_snapshot", 
            "sp_delete_log_shipping_primary_database", "sp_foreign_keys_rowset_rmt", "sp_getschemalock", 
            "xp_readerrorlog", "sp_msadd_compensating_cmd", "sp_msgetmetadatabatch", 
            "sp_addsynctriggers", "sp_msmergeupdatelastsyncinfo", "sp_describe_cursor",
            "sp_msgetonerowlightweight", "sp_dbremove", "sp_dbfixedrolepermission", 
            "sp_mssetsubscriberinfo", "sp_addmergepullsubscription_agent", "sp_setapprole", 
            "sp_msdist_adjust_identity", "sp_monitor", "sp_msfixupbeforeimagetables", 
            "sp_msenum_snapshot_s", "sp_droplogin", "sp_create_plan_guide_from_handle", 
            "sp_mssetconflicttable", "sp_add_log_shipping_secondary_primary", "sp_msadd_subscriber_info",
            "sp_changedbowner", "sp_mssetalertinfo", "sp_adddatatype", "sp_scriptupdproc",
            "sp_revokelogin", "sp_changearticle", "sp_mscleanupmergepublisher_internal", "sp_tableoption",
            "sp_mschangearticleresolver", "sp_msadd_distribution_history", 
            "sp_cdc_generate_wrapper_function", "xp_replposteor", "sp_msuplineageversion", 
            "sp_dropextendedproperty", "sp_msregenerate_mergetriggersprocs", "sp_trace_setstatus", 
            "sp_fulltext_pendingchanges", "sp_msinsertschemachange", "sp_msdummyupdate", 
            "sp_addqreader_agent", "sp_msevaluate_change_membership_for_row", "sp_mstestbit", 
            "sp_msdistribution_counters", "sp_mscomputelastsentgen", "sp_msupdatecachedpeerlsn", 
            "sp_helpdistributor_properties", "sp_articlecolumn", "sp_msdropconstraints", 
            "sp_helpdbfixedrole", "sp_dropdistributiondb", "xp_stopmail", 
            "sp_msrecordsnapshotdeliveryprogress", "xp_sscanf", "sp_msset_sub_guid", "sp_msinsert_identity",
            "sp_activedirectory_start", "sp_msdrop_subscription", "sp_msquery_syncstates",
            "sp_addlogreader_agent", "sp_create_removable", "sp_msenum_merge_subscriptions_90_publisher",
            "sp_msget_max_used_identity", "sp_msmakedynsnapshotvws", 
            "sp_replmonitorhelppublicationthresholds", "sp_invalidate_textptr", "sp_statistics", 
            "sp_scriptpublicationcustomprocs", "sp_cursorclose", "sp_mssqlole65_version", "sp_prepexec",
            "xp_passagentinfo", "sp_msforce_drop_distribution_jobs", "sp_msadd_subscriber_schedule", 
            "sp_addsubscriber_schedule", "sp_mssqlole_version", "sp_deletemergeconflictrow",
            "sp_msprepare_mergearticle", "sp_mspeertopeerfwdingexec", "sp_msenumdeletesmetadata", 
            "sp_msreset_synctran_bit", "sp_statistics_rowset2", "sp_user_counter2", "sp_schemafilter", 
            "xp_revokelogin", "sp_primary_keys_rowset_rmt", "sp_validatemergepublication", 
            "sp_cleanupdbreplication", "sp_scriptsinsproc", "sp_mssetupnosyncsubwithlsnatdist", 
            "sp_msrepl_enumtablecolumninfo", "sp_msenumallsubscriptions", "sp_testlinkedserver", 
            "sp_vupgrade_mergeobjects", "sp_dropdistpublisher", "sp_add_data_file_recover_suspect_db",
            "sp_datatype_info_100", "sp_mstran_ddlrepl", "sp_mssetrowmetadata", "sp_lookupcustomresolver", 
            "sp_addmergefilter", "sp_ihget_loopback_detection", "sp_repldeletequeuedtran", 
            "sp_processlogshippingmonitorsecondary", "sp_batch_params", "sp_columns_rowset2", 
            "sp_usertypes_rowset_rmt", "sp_drop_agent_parameter", "sp_helpextendedproc", 
            "sp_mssetaccesslist", "sp_msenumpartialdeletes", "sp_msaddinitialarticle", 
            "sp_help_agent_parameter", "sp_mscreate_dist_tables", "sp_replddlparser", "sp_msgetpeerlsns",
            "sp_helplogreader_agent", "sp_mspeerapplyresponse", "sp_resyncuniquetable", 
            "sp_setoraclepackageversion", "sp_msrestoresavedforeignkeys", "sp_msrepl_isdbowner", 
            "sp_replmonitorchangepublicationthreshold", "sp_changedistpublisher", "sp_msadd_merge_agent",
            "sp_helpmergelogsettings", "sp_tables", "sp_droppullsubscription", "sp_foreignkeys", 
            "sp_ms_replication_installed", "sp_msgetmaxsnapshottimestamp", "sp_oledb_language", 
            "sp_msadd_snapshot_agent", "sp_help_log_shipping_alert_job", "sp_flush_commit_table",
            "sp_refresh_heterogeneous_publisher", "sp_oageterrorinfo", "sp_sproc_columns_90",
            "sp_msinsertlightweightschemachange", "sp_dbmmonitorchangealert", "sp_columns_100_rowset", 
            "sp_msenum_merge", "sp_getsubscription_status_hsnapshot", "sp_cdc_start_job", "sp_createstats",
            "sp_msdrop_logreader_agent", "sp_oamethod", "sp_table_privileges_rowset", "sp_mspost_auto_proc",
            "sp_addlogin", "sp_msdroparticleconstraints", "sp_droppublisher", "sp_mssub_check_identity",
            "sp_mschange_merge_agent_properties", "sp_dropanonymousagent", "sp_cdc_enable_table", 
            "sp_helpreplicationdboption", "sp_mstran_is_snapshot_required", "sp_mssetcontext_replagent",
            "sp_helpmergearticlecolumn", "xp_msx_enlist", "sp_msdrop_tempgenhistorytable", 
            "sp_mssetlastrecgen", "sp_dropmergelogsettings", "sp_mshelplogreader_agent", 
            "sp_replpostsyncstatus", "sp_fulltext_getdata", "sp_table_type_pkeys", "sp_adddistributor",
            "sp_posttracertoken", "sp_msadd_article", "xp_get_script", "sp_mscleanup_agent_entry",
            "sp_msenum_merge_subscriptions_90_publication", "sp_msadd_repl_error", 
            "sp_msset_snapshot_xact_seqno", "sp_msretrieve_publication_attributes", 
            "sp_table_privileges_rowset_rmt", "sp_indexes_90_rowset_rmt", "sp_msenum_replsqlqueues", 
            "sp_browsemergesnapshotfolder", "sp_tablecollations_90", "sp_mscheck_pub_identity", 
            "sp_unsubscribe", "sp_tables_info_90_rowset", "sp_indexoption", "sp_cursorunprepare", 
            "sp_indexes_rowset2", "sp_enable_sql_debug", "sp_mslocktable", "xp_availablemedia", 
            "sp_http_generate_wsdl_complex", "xp_sysmail_attachment_load", "sp_mssubscription_status", 
            "sp_msdrop_subscription_3rd", "xp_sqlmaint", "sp_msisnonpkukupdateinconflict", 
            "sp_msget_synctran_commands", "sp_msgetdbversion", "sp_fulltext_column", 
            "sp_getagentparameterlist", "sp_msdefer_check", "sp_msreplraiserror", "sp_helppublication",
            "sp_mshasdbaccess", "sp_cdc_drop_job", "sp_mscheck_pull_access", "sp_ihadd_sync_command",
            "sp_msflush_access_cache", "sp_mshelp_replication_table", "sp_register_custom_scripting", 
            "sp_adddistpublisher", "xp_prop_oledb_provider", "sp_mschangeobjectowner", 
            "sp_msenumschemachange", "sp_msaddguidindex", "sp_msgetpubinfo", "sp_repldropcolumn", 
            "sp_catalogs", "sp_dbmmonitorhelpalert", "sp_mscheck_subscription_expiry", 
            "sp_helpsubscriberinfo", "sp_cleanmergelogfiles", "sp_msensure_single_instance", 
            "sp_mssqldmo80_version", "sp_setreplfailovermode", "sp_mschange_originatorid", 
            "sp_msenum_subscriptions", "sp_execute", "sp_cdc_get_ddl_history", 
            "sp_mssetup_partition_groups", "sp_msupdategenerations_afterbcp", 
            "sp_check_log_shipping_monitor_alert", "sp_mstrypurgingoldsnapshotdeliveryprogress", 
            "sp_dropanonymoussubscription", "sp_dbmmonitorchangemonitoring", "sp_publicationsummary", 
            "sp_msmatchkey", "sp_validatelogins", "sp_msdelgenzero", "sp_msmakebatchupdateproc", 
            "sp_user_counter8", "sp_msinit_subscription_agent", "sp_addmessage", 
            "sp_msadd_subscription_3rd", "sp_msunmarkreplinfo", "sp_helpremotelogin", "sp_replflush", 
            "sp_remoteoption", "sp_helpmergeconflictrows", "sp_getpublisherlink", 
            "sp_droplinkedsrvlogin", "sp_constr_col_usage_rowset2", "sp_bcp_dbcmptlevel", 
            "sp_mschange_article", "sp_msregistermergesnappubid", "sp_getapplock", "sp_helpmergelogfiles",
            "xp_loginconfig", "sp_msuploadsupportabilitydata", "sp_mshelp_subscription_status",
            "sp_msget_log_shipping_new_sessionid", "sp_msscriptcustominsproc", 
            "sp_http_generate_wsdl_simple", "sp_helpmergearticle", "sp_msreset_transaction", 
            "sp_delete_log_shipping_primary_secondary", "xp_instance_regremovemultistring", 
            "sp_msrepl_getdistributorinfo", "sp_startpushsubscription_agent", "sp_changesubstatus", 
            "sp_procoption", "sp_enumoledbdatasources", "sp_msispkupdateinconflict", "sp_grantlogin",
            "sp_datatype_info_90", "sp_mshelp_publication", "sp_msagent_retry_stethoscope", 
            "sp_validatecache", "sp_msadd_subscription", "sp_column_privileges_ex", 
            "sp_helpmergealternatepublisher", "sp_mshelptracertokenhistory", "sp_mshaschangeslightweight",
            "sp_changepublication", "sp_resyncexecute", "sp_mscreatelightweightprocstriggersconstraints", 
            "sp_add_log_shipping_primary_secondary", "sp_subscriptionsummary", 
            "sp_help_fulltext_tables_cursor", "sp_msenum_distribution_s", "sp_usertypes_rowset", 
            "sp_procedure_params_90_rowset", "sp_mssetserverproperties", "sp_tablecollations_100", 
            "sp_tables_info_rowset2_64", "sp_mshelpdynamicsnapshotjobatdistributor", 
            "sp_msget_last_transaction", "sp_msuniquename", "sp_mshelpmergeidentity", 
            "sp_msdist_activate_auto_sub", "sp_msget_oledbinfo", "sp_foreign_keys_rowset2",
            "sp_msget_attach_state", "sp_indexes_100_rowset2", "sp_msdbuseraccess", 
            "sp_msadd_merge_history90", "sp_ihscriptidxfile", "sp_mschange_retention", "sp_addtype",
            "sp_trace_setevent", "xp_sqlagent_param", "sp_subscription_cleanup",
            "sp_msaddmergetriggers_internal", "xp_test_mapi_profile", "sp_msreenable_check",
            "sp_msgetsubscriberinfo", "sp_helpxactsetjob", "sp_msscriptdb_worker", "sp_cursoroption",
            "sp_cleanup_log_shipping_history", "sp_who2", "sp_help_log_shipping_monitor_secondary", 
            "sp_estimated_rowsize_reduction_for_vardecimal", "sp_msnonsqlddl", "sp_msmakegeneration",
            "sp_revokedbaccess", "sp_adjustpublisheridentityrange", "sp_check_for_sync_trigger", 
            "sp_add_log_shipping_primary_database", "xp_regenumkeys", "sp_msclear_dynamic_snapshot_location",
            "sp_mscheck_subscription_partition", "sp_mshelpreplicationtriggers",
            "sp_mscreate_tempgenhistorytable", "sp_control_dbmasterkey_password", 
            "sp_mscreatelightweightmultipurposeproc", "sp_tables_info_90_rowset2_64", 
            "sp_mssetreplicaschemaversion", "sp_xp_cmdshell_proxy_account", "sp_msscriptforeignkeyrestore",
            "sp_dropmergepublication", "sp_helpsrvrolemember", "xp_regenumvalues", "sp_oasetproperty", 
            "sp_help_log_shipping_secondary_primary", "sp_addsynctriggerscore", 
            "sp_estimate_data_compression_savings", "sp_msgenerateexpandproc", 
            "sp_mshelpmergedynamicsnapshotjob", "sp_msupdate_singlelogicalrecordmetadata", "sp_helpdb", 
            "sp_delete_log_shipping_alert_job", "sp_getsubscriptiondtspackagename", 
            "sp_vupgrade_replsecurity_metadata", "xp_grantlogin", "xp_subdirs", "sp_msanonymous_status", 
            "sp_fuzzylookuptablemaintenanceinstall", "sp_changeobjectowner", "sp_mstablechecks", 
            "sp_check_subset_filter", "sp_marksubscriptionvalidation", "sp_msgetmetadatabatch90",
            "sp_msloginmappings", "sp_helprolemember", "sp_help_fulltext_catalog_components",
            "sp_sparse_columns_100_rowset", "sp_script_reconciliation_delproc", "sp_columns_100_rowset2",
            "sp_msdrop_merge_agent", "sp_dropsubscriber", "sp_addmergepartition", 
            "sp_setautosapasswordanddisable", "sp_msdrop_subscriber_info", "sp_renamedb", "sp_depends",
            "xp_adsirequest", "sp_serveroption", "sp_msenum_merge_sd", 
            "sp_msreleasesnapshotdeliverysessionlock", "sp_cursorprepare", "xp_sprintf", 
            "sp_msenumpubreferences", "sp_procedures_rowset", "sp_droporphans", "sp_columns_90_rowset",
            "sp_mscreatelightweightinsertproc", "sp_msscriptcustomdelproc", "sp_msscript_pub_upd_trig", 
            "sp_trace_setfilter", "sp_mstablespace", "sp_msuselightweightreplication", 
            "sp_indexes_rowset_rmt", "sp_ih_lr_getcachedata", "sp_spaceused", "sp_msadd_replmergealert",
            "sp_mssetreplicainfo", "sp_statistics_rowset", "sp_mssetconflictscript",
            "sp_msrepl_islastpubinsharedsubscription", "sp_cdc_cleanup_change_table", "xp_create_subdir",
            "sp_mschange_mergearticle", "sp_scriptsubconflicttable", "sp_cdc_dbsnapshotlsn", 
            "sp_msreset_attach_state", "sp_msfetchadjustidentityrange", "sp_mshelpmergeconflictcounts", 
            "sp_mspublicationview", "sp_column_privileges", "sp_helpmergefilter", "sp_msdelrow",
            "sp_msdistributoravailable", "xp_regread", "sp_add_agent_parameter",
            "sp_check_publication_access", "sp_tables_info_90_rowset2", "sp_msgetreplicastate", 
            "sp_msget_min_seqno", "sp_cdc_restoredb", "xp_instance_regdeletekey", "sp_getbindtoken",
            "xp_sysmail_format_query", "sp_replmonitorhelppublication", "sp_check_sync_trigger", 
            "sp_msreplagentjobexists", "sp_helparticlecolumns", "sp_attachsubscription", "sp_ddopen", 
            "sp_msmerge_log_identity_range_allocations", "sp_msget_repl_cmds_anonymous", 
            "sp_msgetsupportabilitysettings", "sp_resetsnapshotdeliveryprogress", "sp_sproc_columns", 
            "sp_msgetconflictinsertproc", "sp_mscheck_snapshot_agent", "sp_indexes_rowset", 
            "sp_msgetalternaterecgens", "sp_mscdc_capture_job", "sp_msadd_logreader_agent", 
            "sp_cursorexecute", "sp_mschange_snapshot_agent_properties", "sp_msrepl_createdatatypemappings",
            "sp_tables_rowset2", "sp_addmergepullsubscription", "sp_resyncmergesubscription", 
            "sp_msenumthirdpartypublicationvendornames", "sp_addmergelogsettings", "sp_replincrementlsn",
            "sp_msadd_merge_history", "sp_helppeerresponses", "sp_msenum_merge_agent_properties", 
            "sp_columns", "sp_msgetlastsentgen", "sp_mshelpfulltextscript", "sp_objectfilegroup", 
            "sp_publication_validation", "xp_get_mapi_default_profile", "sp_msgetalertinfo", 
            "sp_changesubscription", "sp_fuzzylookuptablemaintenanceinvoke", "sp_get_query_template", 
            "sp_helpmergedeleteconflictrows", "sp_syspolicy_subscribe_to_policy_category", "sp_replrestart",
            "sp_msdistpublisher_cleanup", "sp_change_agent_profile", "sp_msgetmakegenerationapplock_90", 
            "sp_msrefresh_anonymous"
        };
        #endregion
        private static TokenType FindTokenType(string token)
        {
            if (Regex.IsMatch(token, @"^\s*N?\'.+?\'\s*$"))
                return TokenType.String;
            if (Regex.IsMatch(token, @"^\s*--.+$"))
                return TokenType.Comment;
            if (Regex.IsMatch(token, @"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/", RegexOptions.Multiline))
                return TokenType.Comment;
            token = token.Trim().ToLower();
            if (Keywords.Contains(token))
                return TokenType.Keyword;
            if (GrayKeywords.Contains(token))
                return TokenType.GKeyword;
            if (Functions.Contains(token))
                return TokenType.Function;
            if (Systems.Contains(token))
                return TokenType.System;
            if (Procedures.Contains(token))
                return TokenType.Procedure;
            return TokenType.Text;
        }
    }
}
