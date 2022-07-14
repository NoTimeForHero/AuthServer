
export interface Company {
  icon: string,
  title: string,
}

const MailRu : Company = {
  icon: '/images/brands/mail_ru.svg',
  title: 'Mail.RU',
}

const Google : Company = {
  icon: '/images/brands/google.svg',
  title: 'Google'
}

const GitHub : Company = {
  icon: '/images/brands/github.png',
  title: 'GitHub'
}

const Yandex : Company = {
  icon: '/images/brands/yandex.png',
  title: 'Яндекс',
}

export const getCompany = (name: string) : Company => {
  return MailRu;
}

export const Companies = {
  MailRu,
  Google,
  GitHub,
  Yandex
}