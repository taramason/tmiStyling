##Get correct subject list depending on type of request
#if($UtilMethods.isSet($!browseCategory))
   #set($browseCat = $categories.getCategoryByName($browseCategory))
   #set($categoryList = $courseswebapi.getAvailableCourseCategories($isCredit, $browseCat))
#else
   #set($categoryList = $courseswebapi.getAvailableCourseCategories($isCredit))
#end

#set($listSize = $categoryList.size())
#set($topRange = $math.ceil($math.div($listSize, 3)))

#if($request.getParameter("location_cat"))
   #set($extraParams = "&location_cat=${UtilMethods.makeHtmlSafe($request.getParameter('location_cat'))}")
#end

#if($request.getParameter("open_only"))
   #set($extraParams = "${extraParams}&open_only=${UtilMethods.makeHtmlSafe($request.getParameter('open_only'))}")
#end

#if($request.getParameter("term_filter"))
   #set($extraParams = "${extraParams}&ampterm_filter=${UtilMethods.makeHtmlSafe($request.getParameter('term_filter'))}")
#end

#set($subjectCatInode = $UtilMethods.makeHtmlSafe($request.getParameter('subject_cat')))
#set($locationCatInode = $UtilMethods.makeHtmlSafe($request.getParameter('location_cat')))

#if ( $UtilMethods.isSet($!browseCat) )
	<ul><li>#if ( $UtilMethods.isSet($subjectCatInode) )<a href="?subject_cat=$!{extraParams}#courselistings">View All</a>#{else}View All#{end}
	</li></ul>
#end

##<ul>
##foreach($cat in $categoryList )
##    <li #if( $UtilMethods.isSet($subjectCatInode) && $subjectCatInode == $cat.get("category_inode")) class="active" #end>
##		<a href="?subject_cat=${cat.get("category_inode")}$!{extraParams}#courselistings">${cat.get("category_name")}</a>
##	</li>
##end
##</ul>	



<div class="floated">
	<div class="sub-column-third">			
		<ul class="content-list">
		#set ( $loopCount = 0 )
		#foreach($cat in $categoryList )
			<li #if( $UtilMethods.isSet($subjectCatInode) && $subjectCatInode == $cat.get("category_inode")) class="active" #end>
				<a href="?subject_cat=${cat.get("category_inode")}$!{extraParams}#courselistings">${cat.get("category_name")}</a>
			</li>
		#end
		</ul>		
	</div>
</div>
##get courses

#if($UtilMethods.isSet($!browseCategory))
   #set($browseCat = "$categories.getCategoryByName($browseCategory).inode")
#else
    #set($browseCat = "")
#end

#set($showOnlyScheduled = $request.getParameter('open_only'))
#if($showOnlyScheduled)
	#if($showOnlyScheduled.equals('true'))
		#set($showOnlyScheduled = true)
	#else
		#set($showOnlyScheduled = false)
	#end
#else
	#set($showOnlyScheduled = false)
#end

#set($page = $request.getParameter('page'))
#if(!$UtilMethods.isSet($page))
	#set($page = 1)
#else
	#set($page = $math.toInteger($page))
#end

#set($termSelect = $UtilMethods.makeHtmlSafe($request.getParameter('term_filter')))
#if(!$UtilMethods.isSet($termSelect))
	#set($termSelect = $null.getNull() )
#end

##Get correct course list
#if ($UtilMethods.isSet($!browseCat))
  #set($courses = $courseswebapi.getCoursesBrowseListing($subjectCatInode,$browseCat, $showOnlyScheduled, $isCredit, $locationCatInode, $termSelect))
#elseif ($isCredit && ($UtilMethods.isSet($extraParams) || $UtilMethods.isSet($subjectCatInode)) )
  #set($courses = $courseswebapi.getCreditCourseListing($subjectCatInode, $showOnlyScheduled, $locationCatInode, $termSelect))
#elseif ( !$isCredit && ($UtilMethods.isSet($extraParams) || $UtilMethods.isSet($subjectCatInode)) )
  #set($courses = $courseswebapi.getNoncreditCourseListing($subjectCatInode, $showOnlyScheduled, $locationCatInode, $termSelect))
#end

<!-- <script type="text/javascript">
	console.log("$extraParams");
</script> -->