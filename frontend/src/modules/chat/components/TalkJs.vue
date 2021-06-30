<template>
  <div id="talkjs-container" style="width: 90%; margin: 30px; height: 400px">
    <i>Loading chat...</i>
  </div>
</template>

<script lang='ts'>
import { defineComponent } from "vue";
import Talk from "talkjs";

export default defineComponent({
  name: "TalkJs",
  mounted() {
    Talk.ready.then(function () {
      const me = new Talk.User({
        id: parseInt((Math.random() * 500000).toString()).toString(),
        name: "Alice",
        email: "demo@talkjs.com",
        photoUrl: "https://demo.talkjs.com/img/alice.jpg",
        welcomeMessage: "Hey there! How are you? :-)",
        role: "booker",
      });

      const talkSession = new Talk.Session({
        appId: "Hku1c4Pt",
        me: me,
      });

      const other = new Talk.User({
        id: parseInt((Math.random() * 500000).toString()).toString(),
        name: "Sebastian",
        email: "demo@talkjs.com",
        photoUrl: "https://demo.talkjs.com/img/sebastian.jpg",
        welcomeMessage: "Hey, how can I help?",
        role: "booker",
      });

      const conversation = talkSession.getOrCreateConversation(
        Talk.oneOnOneId(me.id, other.id)
      );
      conversation.setParticipant(me);
      conversation.setParticipant(other);
      const inbox = talkSession.createInbox({ selected: conversation });
      inbox.mount(document.getElementById("talkjs-container"));
    });
  },
});
</script>
